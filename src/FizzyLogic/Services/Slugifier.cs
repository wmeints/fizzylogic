using System.Text;
using System.Text.RegularExpressions;

namespace FizzyLogic.Services
{
    public class Slugifier
    {
        private static readonly Regex NonAlphaNumericPattern = new Regex("[^a-z0-9]+");
        private static readonly Regex DashesPattern = new Regex("-+");
        
        public string Process(string title)
        {
            var slug = DashesPattern.Replace(NonAlphaNumericPattern.Replace(title.ToLowerInvariant(),"-"),"-");

            if (slug.EndsWith("-"))
            {
                slug = slug.Substring(0, slug.Length - 1);
            }

            return slug;
        }
    }
}