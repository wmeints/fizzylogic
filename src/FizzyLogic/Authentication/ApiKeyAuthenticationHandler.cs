namespace FizzyLogic.Authentication
{
    using Data;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    /// <summary>
    /// Authenticates requests using an API key
    /// </summary>
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Initializes a new instance of <see cref="ApiKeyAuthenticationHandler"/>
        /// </summary>
        /// <param name="options">Options for the authentication handler.</param>
        /// <param name="logger">Logger for the authentication handler.</param>
        /// <param name="encoder">Encoder used by the authentication handler.</param>
        /// <param name="clock">System clock.</param>
        /// <param name="signInManager">Sign in manager for creating the user principal.</param>
        /// <param name="applicationDbContext">DbContext for locating the super user.</param>
        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext) 
            : base(options, logger, encoder, clock)
        {
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // We only validate an API key if it is present.
            // This may be something that should be handled by another authentication scheme handler.
            if (!Request.Headers.TryGetValue("X-ApiKey", out var apiKeyHeaderValues))
            {
                return AuthenticateResult.NoResult();
            }

            if (!string.IsNullOrEmpty(apiKeyHeaderValues) && apiKeyHeaderValues == Options.ApiKey)
            {
                // When you login using an API token we're identifying you as the super user in the database.
                // Note that we have only one user in the database :-)
                var user = await _applicationDbContext.Users.FirstOrDefaultAsync();
                var principal = await _signInManager.CreateUserPrincipalAsync(user);

                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Unauthorized");
        }
    }
}