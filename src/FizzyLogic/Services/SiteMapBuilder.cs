namespace FizzyLogic.Services
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    public class SiteMapBuilder
    {
        private readonly XDocument _document;
        private readonly XElement _rootElement;

        public SiteMapBuilder()
        {
            _rootElement = new XElement(XName.Get("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9"));
            _document = new XDocument(_rootElement);
        }

        public void WithLocation(string url, DateTime? lastModified, ChangeFrequency? changeFrequency)
        {
            var urlElement = new XElement(XName.Get("url", "http://www.sitemaps.org/schemas/sitemap/0.9"));

            urlElement.Add(new XElement(XName.Get("loc", "http://www.sitemaps.org/schemas/sitemap/0.9"), url));

            if (lastModified != null)
            {
                urlElement.Add(
                    new XElement(XName.Get("lastmod", "http://www.sitemaps.org/schemas/sitemap/0.9"),
                    lastModified.Value.ToString("yyyy-M-d")));
            }

            if (changeFrequency != null)
            {
                urlElement.Add(new XElement(XName.Get("changefreq", "http://www.sitemaps.org/schemas/sitemap/0.9"), changeFrequency switch
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

        public string Build()
        {
            var outputBuilder = new StringBuilder();
            var xmlWriter = new XmlTextWriter(new StringWriter(outputBuilder));

            _document.WriteTo(xmlWriter);

            return outputBuilder.ToString();
        }
    }
}