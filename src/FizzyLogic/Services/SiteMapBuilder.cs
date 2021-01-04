namespace FizzyLogic.Services
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Provides utility functions to build a sitemap for the website.
    /// </summary>
    public class SiteMapBuilder
    {
        public const string Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9";

        private readonly XDocument _document;
        private readonly XElement _rootElement;

        /// <summary>
        /// Initializes a new instance of <see cref="SiteMapBuilder"/>.
        /// </summary>
        public SiteMapBuilder()
        {
            _rootElement = new XElement(XName.Get("urlset", Namespace));
            _document = new XDocument(_rootElement);
        }

        /// <summary>
        /// Includes a location in the sitemap.
        /// </summary>
        /// <param name="url">URL for the sitemap location.</param>
        /// <param name="lastModified">Optional date the item was last changed.</param>
        /// <param name="changeFrequency">Optional setting for update frequency.</param>
        public void WithLocation(string url, DateTime? lastModified, ChangeFrequency? changeFrequency)
        {
            var urlElement = new XElement(XName.Get("url", Namespace));

            urlElement.Add(new XElement(XName.Get("loc", Namespace), url));

            if (lastModified != null)
            {
                urlElement.Add(
                    new XElement(XName.Get("lastmod", Namespace),
                    lastModified.Value.ToString("yyyy-M-d")));
            }

            if (changeFrequency != null)
            {
                urlElement.Add(new XElement(XName.Get("changefreq", Namespace), changeFrequency switch
                {
                    ChangeFrequency.Always => "always",
                    ChangeFrequency.Hourly => "hourly",
                    ChangeFrequency.Daily => "daily",
                    ChangeFrequency.Weekly => "weekly",
                    ChangeFrequency.Monthly => "monthly",
                    ChangeFrequency.Never => "never",
                    ChangeFrequency.Yearly => "yearly",
                    _ => null
                }));
            }

            _rootElement.Add(urlElement);
        }

        /// <summary>
        /// Generates the sitemap.xml content.
        /// </summary>
        /// <returns>Returns a string containing the sitemap.xml content.</returns>
        public string Build()
        {
            var outputBuilder = new StringBuilder();
            var xmlWriter = new XmlTextWriter(new StringWriter(outputBuilder));

            _document.WriteTo(xmlWriter);

            return outputBuilder.ToString();
        }
    }
}