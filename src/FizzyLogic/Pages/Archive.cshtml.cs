using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FizzyLogic.Data;
using FizzyLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FizzyLogic.Pages
{
    public class ArchivePageModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ArchivePageModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IEnumerable<Article> Articles { get; set; }
        public int PageIndex { get; set; }
        
        public async Task<IActionResult> OnGet(int pageIndex = 1)
        {
            Articles = await _applicationDbContext.Articles
                .Include(x=>x.Category)
                .Where(x=>x.DatePublished != null)
                .OrderByDescending(x => x.DatePublished)
                .Skip((pageIndex - 1) * 12)
                .Take(12).ToListAsync();

            PageIndex = pageIndex + 1;
            
            return Page();
        }
    }
}