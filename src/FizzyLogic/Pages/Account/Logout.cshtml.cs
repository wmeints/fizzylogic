using System.Threading.Tasks;
using FizzyLogic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FizzyLogic.Pages.Account
{
    public class LogoutPageModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LogoutPageModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Admin/Index");
        }
    }
}