namespace FizzyLogic.Pages
{
    using System.Threading.Tasks;
    using FizzyLogic.Data;
    using FizzyLogic.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    public class ArticlePageModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ArticlePageModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Article Article { get; set; }

        public async Task<IActionResult> OnGetAsync(string slug)
        {
            Article = await _applicationDbContext.Articles
                .Include(x => x.Category)
                .SingleOrDefaultAsync(x => x.Slug == slug && x.DatePublished != null);

            return Article == null ? NotFound() : Page();
        }
    }
}