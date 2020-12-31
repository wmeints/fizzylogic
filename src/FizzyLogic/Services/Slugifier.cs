namespace FizzyLogic.Services
{
    using System.Text.RegularExpressions;
    
    /// <summary>
    /// Provides a technique to turn title information into a nice url.
    /// </summary>
    public class Slugifier
    {
        private static readonly Regex NonAlphaNumericPattern = new Regex("[^a-z0-9]+");
        private static readonly Regex DashesPattern = new Regex("-+");

        /// <summary>
        /// Processes a title into a slug.
        /// </summary>
        /// <param name="title">Raw title for an article or category.</param>
        /// <returns>Returns the slugified version of the title.</returns>
        public string Process(string title)
        {
            return DashesPattern.Replace(NonAlphaNumericPattern.Replace(title.ToLowerInvariant(), "-"), "-").TrimEnd('-');
        }
    }
}