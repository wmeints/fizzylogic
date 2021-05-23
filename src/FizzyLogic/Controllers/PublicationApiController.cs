namespace FizzyLogic.Controllers
{
    using Data;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using Forms;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Models;
    using Services;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Validators;

    /// <summary>
    /// Exposes a couple of endpoints for publishing content from vscode.
    /// </summary>
    [ApiController]
    [Route("/api/posts")]
    [Authorize(AuthenticationSchemes = "Identity.Application,ApiKey")]
    public class PublicationApiController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<PublicationApiController> _logger;
        private readonly IClock _clock;
        private readonly Slugifier _slugifier;

        /// <summary>
        /// Initializes a new instance of <see cref="PublicationApiController"/>.
        /// </summary>
        /// <param name="applicationDbContext"></param>
        /// <param name="slugifier"></param>
        /// <param name="logger"></param>
        /// <param name="clock"></param>
        public PublicationApiController(ApplicationDbContext applicationDbContext,
            ILogger<PublicationApiController> logger, IClock clock,
            Slugifier slugifier)
        {
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
        [HttpGet]
        public async Task<IActionResult> ListPosts(PublicationStatus status, int page = 0)
        {
            IQueryable<Article> posts = _applicationDbContext.Articles.OrderBy(x => x.Title);

#pragma warning disable IDE0072

            posts = status switch
            {
                PublicationStatus.Draft => posts.Where(x => x.DatePublished == null),
                PublicationStatus.Published => posts.Where(x => x.DatePublished != null),
                _ => posts
            };

#pragma warning restore IDE0072

            var items = await posts.Skip(page * 20).Take(20).ToListAsync();
            var totalItemCount = await posts.CountAsync();

            return Ok(new PagedResult<Article>(items, page, 20, totalItemCount));
        }

        /// <summary>
        /// Publishes an article on the website.
        /// </summary>
        /// <param name="form">The content of the article to publish.</param>
        /// <returns>The outcome of the publish action.</returns>
        [HttpPost]
        public async Task<IActionResult> Publish(PublishArticleForm form)
        {
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

            return form.Id != null ? await UpdateExistingArticle(form) : await PublishNewArticle(form);
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

            _ = await _applicationDbContext.SaveChangesAsync();

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

            _ = await _applicationDbContext.Articles.AddAsync(article);
            _ = await _applicationDbContext.SaveChangesAsync();

            return Ok(article);
        }
    }
}