namespace FizzyLogic.Services
{
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides a technique to turn title information into a nice url.
    /// </summary>
    public class Slugifier
    {
        private static readonly Regex NonAlphaNumericPattern = new("[^a-z0-9]+");
        private static readonly Regex DashesPattern = new("-+");

        /// <summary>
        /// Processes a title into a slug.
        /// </summary>
        /// <param name="title">Raw title for an article or category.</param>
        /// <returns>Returns the slugified version of the title.</returns>
        public string Process(string title)
        {
            return DashesPattern
                .Replace(NonAlphaNumericPattern.Replace(NormalizeString(title).ToLower(), "-"), "-")
                .TrimEnd('-');
        }

        private static string NormalizeString(string text)
        {
            var outputBuilder = new StringBuilder();
            var normalizedText = text.Normalize(NormalizationForm.FormD);

            foreach (var c in normalizedText)
            {
                if (char.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    _ = outputBuilder.Append(c);
                }
            }

            return outputBuilder.ToString();
        }
    }
}