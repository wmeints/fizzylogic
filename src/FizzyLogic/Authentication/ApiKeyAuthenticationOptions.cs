// unset

namespace FizzyLogic.Authentication
{
    using Microsoft.AspNetCore.Authentication;

    /// <summary>
    /// Options for the API key authentication scheme.
    /// </summary>
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// Gets or sets the API key that users can use to authenticate with.
        /// </summary>
        public string ApiKey { get; set; }
    }
}