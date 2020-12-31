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
    /// Page model for the archive page.
    /// </summary>
    public class ArchivePageModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        /// <summary>
        /// Initializes a new instance of <see cref="ArchivePageModel"/>.
        /// </summary>
        /// <param name="applicationDbContext">DbContext to use for loading the data.</param>
        public ArchivePageModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Gets or sets the articles to show on the page.
        /// </summary>
        public IEnumerable<Article> Articles { get; set; }

        /// <summary>
        /// Gets or sets the page index to render.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; }

        /// <summary>
        /// Handles the GET operation for the page.
        /// </summary>
        public async Task<IActionResult> OnGet()
        {
            Articles = await _applicationDbContext.Articles
                .Include(x => x.Category)
                .Where(x => x.DatePublished != null)
                .OrderByDescending(x => x.DatePublished)
                .Skip((PageIndex - 1) * 12)
                .Take(12).ToListAsync();

            return Page();
        }
    }
}