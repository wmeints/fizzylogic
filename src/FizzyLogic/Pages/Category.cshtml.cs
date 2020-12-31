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

    public class CategoryPageModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CategoryPageModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Category Category { get; set; }

        public IEnumerable<Article> Articles { get; set; }

        public async Task<IActionResult> OnGetAsync(string slug)
        {
            Category = await _applicationDbContext.Categories.SingleOrDefaultAsync(x => x.Slug == slug);

            if (Category == null)
            {
                return NotFound();
            }

            Articles = await _applicationDbContext.Articles
                .Where(x => x.Category.Slug == slug && x.DatePublished != null)
                .OrderByDescending(x => x.DatePublished)
                .Take(12)
                .ToListAsync();

            return Page();
        }
    }
}