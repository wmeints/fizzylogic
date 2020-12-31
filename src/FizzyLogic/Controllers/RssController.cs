namespace FizzyLogic.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using FizzyLogic.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Provides the /rss endpoint for the website.
    /// </summary>
    [ApiController]
    public class RssController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        /// <summary>
        /// Initializes a new instance of <see cref="RssController"/>.
        /// </summary>
        /// <param name="applicationDbContext">DbContext for retrieving article content.</param>
        public RssController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Retrieves the RSS feed for the website containing the last 50 articles.
        /// </summary>
        /// <returns>Returns OK with the RSS content.</returns>
        [HttpGet]
        [Route("/rss")]
        public async Task<IActionResult> Index()
        {
            var contentItems = await _applicationDbContext.Articles
                .Include(x => x.Category)
                .Where(x => x.DatePublished != null)
                .OrderBy(x => x.DatePublished)
                .Take(50)
                .ToListAsync();

            var feedItems = new List<SyndicationItem>();

            foreach (var contentItem in contentItems)
            {
                var alternativeUrl = new Uri(
                    $"https://fizzylogic.nl/{contentItem.DatePublished.Value.Year:0000}/" +
                    $"{contentItem.DatePublished.Value.Month:00}/" +
                    $"{contentItem.DatePublished.Value.Day:00}/" +
                    $"{contentItem.Slug}");

                var feedItemContent = new TextSyndicationContent(contentItem.Html, TextSyndicationContentKind.Html);

                var feedItem = new SyndicationItem(contentItem.Title, feedItemContent, alternativeUrl,
                    contentItem.Id.ToString(), contentItem.DatePublished.Value);

                feedItems.Add(feedItem);
            }

            var feed = new SyndicationFeed(
                "Willem's Fizzy Logic",
                "Random talk about machine learning and other development efforts",
                new Uri("https://fizzylogic.nl/"),
                feedItems);

            var outputBuilder = new StringBuilder();
            var outputWriter = new StringWriter(outputBuilder);
            var xmlWriter = new XmlTextWriter(outputWriter);
            var feedFormatter = new Rss20FeedFormatter(feed);

            feedFormatter.WriteTo(xmlWriter);

            return Content(outputBuilder.ToString(), "application/rss+xml; charset=utf-8");
        }
    }
}