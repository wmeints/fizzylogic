namespace FizzyLogic.Pages.Account
{
    using System.Threading.Tasks;
    using FizzyLogic.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    /// <summary>
    /// Page model for the logout page.
    /// </summary>
    public class LogoutPageModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Initializes a new instance of <see cref="LogoutPageModel"/>.
        /// </summary>
        /// <param name="signInManager">Sign-in manager to use for logging out.</param>
        public LogoutPageModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        /// <summary>
        /// Handles the GET operation on the page.
        /// </summary>
        public async Task<IActionResult> OnGetAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Admin/Index");
        }
    }
}