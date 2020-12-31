namespace FizzyLogic.Pages.Account
{
    using System.Threading.Tasks;
    using FizzyLogic.Forms;
    using FizzyLogic.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    /// <summary>
    /// Page model for the login page.
    /// </summary>
    public class LoginPageModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Initializes a new instance of <see cref="LoginPageModel"/>.
        /// </summary>
        /// <param name="signInManager">The sign-in manager to use for logging in.</param>
        public LoginPageModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        /// <summary>
        /// Gets or sets the URL to return to after logging in.
        /// </summary>
        [BindProperty]
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets the login information to use.
        /// </summary>
        [BindProperty]
        public LoginForm Input { get; set; }

        /// <summary>
        /// Handles the GET operation on the page.
        /// </summary>
        public IActionResult OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            return Page();
        }

        /// <summary>
        /// Handles the POST operation on the page.
        /// </summary>
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