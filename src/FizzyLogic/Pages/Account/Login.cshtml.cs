using System.Threading.Tasks;
using FizzyLogic.Forms;
using FizzyLogic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FizzyLogic.Pages.Account
{
    public class LoginPageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginPageModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public string ReturnUrl { get; set; }
        
        [BindProperty]
        public LoginForm Input { get; set; }

        public IActionResult OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    userName: Input.EmailAddress,
                    password: Input.Password,
                    isPersistent: false,
                    lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    return LocalRedirect(ReturnUrl ?? "/Admin/Index");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username/password combination.");
                }
            }

            return Page();
        }
    }
}