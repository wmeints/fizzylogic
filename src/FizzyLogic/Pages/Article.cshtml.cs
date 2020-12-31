namespace FizzyLogic.Pages
{
    using System.Threading.Tasks;
    using FizzyLogic.Data;
    using FizzyLogic.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Page model for the article page.
    /// </summary>
    public class ArticlePageModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        /// <summary>
        /// Initializes a new instance of <see cref="ArticlePageModel"/>.
        /// </summary>
        /// <param name="applicationDbContext">DbContext to load data.</param>
        public ArticlePageModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Gets or sets the article to render.
        /// </summary>
        public Article Article { get; set; }

        /// <summary>
        /// Handles the GET operation for the page.
        /// </summary>
        /// <param name="slug">The slug for the page.</param>
        public async Task<IActionResult> OnGetAsync(string slug)
        {
            Article = await _applicationDbContext.Articles
                .Include(x => x.Category)
                .SingleOrDefaultAsync(x => x.Slug == slug && x.DatePublished != null);

            return Article == null ? NotFound() : Page();
        }
    }
}