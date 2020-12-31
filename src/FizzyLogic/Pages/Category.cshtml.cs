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

    /// <summary>
    /// Page model for the category page.
    /// </summary>
    public class CategoryPageModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        /// <summary>
        /// Initializes a new instance of <see cref="CategoryPageModel"/>.
        /// </summary>
        /// <param name="applicationDbContext">DbContext for loading data.</param>
        public CategoryPageModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Gets or sets the category to render.
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Gets or sets the articles for the article.
        /// </summary>
        public IEnumerable<Article> Articles { get; set; }

        /// <summary>
        /// Handles the GET operation for the page.
        /// </summary>
        /// <param name="slug">Slug for the category.</param>
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