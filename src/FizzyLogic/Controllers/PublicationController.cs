namespace FizzyLogic.Controllers
{
    using Configuration;
    using Data;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using Forms;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models;
    using Services;
    using System;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
    using Validators;

    /// <summary>
    /// Exposes a couple of endpoints for publishing content from vscode.
    /// </summary>
    public class PublicationApiController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IOptions<PublicationApiOptions> _options;
        private readonly ILogger<PublicationApiController> _logger;
        private readonly IClock _clock;
        private readonly Slugifier _slugifier;

        /// <summary>
        /// Initializes a new instance of <see cref="PublicationApiController"/>.
        /// </summary>
        /// <param name="applicationDbContext"></param>
        /// <param name="options"></param>
        /// <param name="slugifier"></param>
        /// <param name="logger"></param>
        /// <param name="clock"></param>
        public PublicationApiController(ApplicationDbContext applicationDbContext,
            IOptions<PublicationApiOptions> options, ILogger<PublicationApiController> logger, IClock clock,
            Slugifier slugifier)
        {
            _options = options;
            _applicationDbContext = applicationDbContext;
            _logger = logger;
            _clock = clock;
            _slugifier = slugifier;
        }

        /// <summary>
        /// Lists all posts on the blog, filtered by status.
        /// </summary>
        /// <param name="page">Index of the page to retrieve.</param>
        /// <param name="status">The publication status of the posts.</param>
        /// <returns>Returns the list of posts.</returns>
        [HttpGet("/api/posts")]
        public async Task<IActionResult> ListPosts(PublicationStatus status, int page = 0)
        {
            if (!ValidateApiKey())
            {
                return Unauthorized();
            }

            IQueryable<Article> posts = _applicationDbContext.Articles.OrderBy(x => x.Title);

            if (status == PublicationStatus.Draft)
                posts = posts.Where(x => x.DatePublished == null);
            else if (status == PublicationStatus.Published)
                posts = posts.Where(x => x.DatePublished != null);

            var items = await posts.Skip(page * 20).Take(20).ToListAsync();
            var totalItemCount = await posts.CountAsync();

            return Ok(new PagedResult<Article>(items, page, 20, totalItemCount));
        }

        /// <summary>
        /// Publishes an article on the website.
        /// </summary>
        /// <param name="form">The content of the article to publish.</param>
        /// <returns>The outcome of the publish action.</returns>
        public async Task<IActionResult> Publish(PublishArticleForm form)
        {
            if (!ValidateApiKey())
            {
                return Unauthorized();
            }

            var validator = new PublishArticleFormValidator(_applicationDbContext);

            var validationResults = await validator.ValidateAsync(
                form, options => options
                    .IncludeRuleSets(form.Id != null ? "Updating" : "Creating")
                    .IncludeRulesNotInRuleSet());

            validationResults.AddToModelState(ModelState, "");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(form.Excerpt))
            {
                var plainTextContent = MarkdownConverter.ConvertToText(form.Markdown);
                form.Excerpt = plainTextContent[..Math.Min(plainTextContent.Length, 250)];
            }

            if (string.IsNullOrEmpty(form.Slug))
            {
                form.Slug = _slugifier.Process(form.Title);
            }

            if (form.Id != null)
            {
                return await UpdateExistingArticle(form);
            }

            return await PublishNewArticle(form);
        }

        private async Task<IActionResult> UpdateExistingArticle(PublishArticleForm form)
        {
            var article = await _applicationDbContext.Articles
                .SingleOrDefaultAsync(x => x.Id == form.Id);

            var category = await _applicationDbContext.Categories
                .SingleOrDefaultAsync(x => x.Slug == form.Category);

            article.Title = form.Title;
            article.Markdown = form.Markdown;
            article.Excerpt = form.Excerpt;
            article.Category = category;
            article.FeaturedImage = form.FeaturedImage;
            article.DateModified = _clock.UtcNow;
            article.Slug = form.Slug;

            await _applicationDbContext.SaveChangesAsync();

            return Ok(article);
        }

        private async Task<IActionResult> PublishNewArticle(PublishArticleForm form)
        {
            var category = await _applicationDbContext.Categories.SingleOrDefaultAsync(x => x.Slug == form.Category);
            var author = await _applicationDbContext.Users.FirstAsync();

            var article = new Article
            {
                Title = form.Title,
                Markdown = form.Markdown,
                Author = author,
                Category = category,
                DateCreated = _clock.UtcNow,
                DatePublished = !form.Draft ? _clock.UtcNow : null,
                FeaturedImage = form.FeaturedImage,
                Slug = form.Slug
            };

            await _applicationDbContext.Articles.AddAsync(article);
            await _applicationDbContext.SaveChangesAsync();

            return Ok(article);
        }

        private bool ValidateApiKey()
        {
            if (string.IsNullOrEmpty(_options.Value.ApiKey))
            {
                _logger.LogWarning(
                    "No API key specified, you need to configure an API key before you can use this feature");

                return false;
            }

            if (Request.Headers.TryGetValue("X-ApiKey", out var keyValues))
            {
                if (keyValues.Contains(_options.Value.ApiKey))
                {
                    return true;
                }
            }

            return false;
        }
    }
}