namespace FizzyLogic.Services
{
    using System.Text.RegularExpressions;

    public class Slugifier
    {
        private static readonly Regex NonAlphaNumericPattern = new Regex("[^a-z0-9]+");
        private static readonly Regex DashesPattern = new Regex("-+");

        public string Process(string title)
        {
            return DashesPattern.Replace(NonAlphaNumericPattern.Replace(title.ToLowerInvariant(), "-"), "-").TrimEnd('-');
        }
    }
}