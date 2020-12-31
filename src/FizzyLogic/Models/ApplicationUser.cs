namespace FizzyLogic.Models
{
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Every user that has admin access is modeled using this class.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the profile picture for the user.
        /// </summary>
        public string ProfilePicture { get; set; }
    }
}