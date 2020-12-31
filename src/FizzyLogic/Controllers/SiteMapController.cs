namespace FizzyLogic.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using FizzyLogic.Data;
    using FizzyLogic.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    public class SiteMapController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public SiteMapController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

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