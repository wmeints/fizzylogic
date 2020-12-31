namespace FizzyLogic.Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FizzyLogic.Data;
    using FizzyLogic.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    public class IndexPageModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public IndexPageModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IEnumerable<Article> Articles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Articles = await _applicationDbContext.Articles
                .Include(x => x.Category)
                .Where(x => x.DatePublished != null)
                .OrderByDescending(x => x.DatePublished)
                .Take(12)
                .ToListAsync();

            return Page();
        }
    }
}