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

    public class CategoryArchivePageModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CategoryArchivePageModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public int PageIndex { get; set; }

        public Category Category { get; set; }

        public IEnumerable<Article> Articles { get; set; }

        public async Task<IActionResult> OnGetAsync(string slug, int pageIndex = 1)
        {
            PageIndex = pageIndex;
            Category = await _applicationDbContext.Categories.SingleOrDefaultAsync(x => x.Slug == slug);

            if (Category == null)
            {
                return NotFound();
            }

            Articles = await _applicationDbContext.Articles
                .Where(x => x.Category.Slug == slug && x.DatePublished != null)
                .OrderByDescending(x => x.DatePublished)
                .Skip((pageIndex - 1) * 12)
                .Take(12)
                .ToListAsync();

            return Page();
        }
    }
}