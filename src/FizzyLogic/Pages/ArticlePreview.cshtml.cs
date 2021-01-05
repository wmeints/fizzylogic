namespace FizzyLogic.Pages
{
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class ArticlePreviewPageModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        /// <summary>
        /// Initializes a new instance of <see cref="ArticlePreviewPageModel"/>.
        /// </summary>
        /// <param name="applicationDbContext">DbContext to load data.</param>
        public ArticlePreviewPageModel(ApplicationDbContext applicationDbContext)
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
        /// <param name="id">The ID for the article.</param>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Article = await _applicationDbContext.Articles
                .Include(x => x.Category)
                .SingleOrDefaultAsync(x => x.Id == id);

            return Article == null ? NotFound() : Page();
        }
    }
}