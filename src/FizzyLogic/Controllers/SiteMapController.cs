namespace FizzyLogic.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using FizzyLogic.Data;
    using FizzyLogic.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Provides /sitemap.xml endpoint for the website.
    /// </summary>
    [ApiController]
    public class SiteMapController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        /// <summary>
        /// Initializes a new instance of <see cref="SiteMapController"/>.
        /// </summary>
        /// <param name="applicationDbContext">DbContext to retrieve the content of the website.</param>
        public SiteMapController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Renders the sitemap for the website.
        /// </summary>
        /// <returns>Returns OK with the sitemap.xml content.</returns>
        [HttpGet]
        [Route("/sitemap.xml")]
        [ResponseCache(Duration = 86400)]
        public async Task<IActionResult> Index()
        {
            var siteMapBuilder = new SiteMapBuilder();

            siteMapBuilder.WithLocation("/", null, null);
            siteMapBuilder.WithLocation("/about", null, null);

#pragma warning disable IDE0050

            var articles = await _applicationDbContext.Articles
                .Where(x => x.DatePublished != null)
                .OrderByDescending(x => x.DatePublished)
                .Select(x => new { x.Slug, x.DatePublished })
                .ToListAsync();

#pragma warning restore IDE0050

            foreach (var article in articles)
            {
                var itemUrl =
                    $"/{article.DatePublished.Value.Year:0000}/" +
                    $"{article.DatePublished.Value.Month:00}/" +
                    $"{article.DatePublished.Value.Day:00}/" +
                    $"{article.Slug}";

                siteMapBuilder.WithLocation(itemUrl, article.DatePublished.Value, ChangeFrequency.Monthly);
            }

            return Content(siteMapBuilder.Build(), "application/xml");
        }
    }
}