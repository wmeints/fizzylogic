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
    /// Page model for the homepage.
    /// </summary>
    public class IndexPageModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        /// <summary>
        /// Initializes a new instance of <see cref="IndexPageModel"/>.
        /// </summary>
        /// <param name="applicationDbContext">DbContext to load data.</param>
        public IndexPageModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Gets or sets the articles to render.
        /// </summary>
        public IEnumerable<Article> Articles { get; set; }

        /// <summary>
        /// Handles the GET operation on the page.
        /// </summary>
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