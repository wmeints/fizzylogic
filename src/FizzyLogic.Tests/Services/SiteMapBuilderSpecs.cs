namespace FizzyLogic.Tests.Services
{
    using Chill;
    using FizzyLogic.Services;
    using FluentAssertions;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Xunit;

    public class WhenGeneratingBasicSiteMap : GivenSubject<SiteMapBuilder, string>
    {
        public WhenGeneratingBasicSiteMap()
        {
            WithSubject(resolver => new SiteMapBuilder());
            When(() =>
            {
                Subject.WithLocation("/", null, null);
                return Subject.Build();
            });
        }

        [Fact]
        public void ThenNoUpdateFrequencyIsSpecified()
        {
            var sitemap = XDocument.Load(new StringReader(Result));

            var urlElement = sitemap.Descendants(XName.Get("url", SiteMapBuilder.Namespace)).SingleOrDefault().Should().NotBeNull();
            _ = urlElement.And.Subject.Element(XName.Get("changefreq", SiteMapBuilder.Namespace)).Should().BeNull();
        }

        [Fact]
        public void ThenNoLastChangedIsSpecified()
        {
            var sitemap = XDocument.Load(new StringReader(Result));

            var urlElement = sitemap.Descendants(XName.Get("url", SiteMapBuilder.Namespace)).SingleOrDefault().Should().NotBeNull();
            _ = urlElement.And.Subject.Element(XName.Get("lastmod", SiteMapBuilder.Namespace)).Should().BeNull();
        }

        [Fact]
        public void ThenTheUrlIsSpecified()
        {
            var sitemap = XDocument.Load(new StringReader(Result));

            var locationElement = new XElement(XName.Get("loc", SiteMapBuilder.Namespace), "/");

            var urlElement = sitemap.Descendants(XName.Get("url", SiteMapBuilder.Namespace)).SingleOrDefault().Should().NotBeNull();
            _ = urlElement.And.Subject.Element(XName.Get("loc", SiteMapBuilder.Namespace)).Should().Be(locationElement);
        }
    }

    public class WhenGeneratingAComplexSiteMap : GivenSubject<SiteMapBuilder, string>
    {
        public WhenGeneratingAComplexSiteMap()
        {
            WithSubject(resolver => new SiteMapBuilder());
            When(() =>
            {
                Subject.WithLocation("/", new System.DateTime(2020, 1, 1), ChangeFrequency.Monthly);
                return Subject.Build();
            });
        }

        [Fact]
        public void ThenNoUpdateFrequencyIsSpecified()
        {
            var sitemap = XDocument.Load(new StringReader(Result));

            var changeFrequencyElement = new XElement(XName.Get("changefreq", SiteMapBuilder.Namespace), "monthly");

            var urlElement = sitemap.Descendants(XName.Get("url", SiteMapBuilder.Namespace)).SingleOrDefault().Should().NotBeNull();
            _ = urlElement.And.Subject.Element(XName.Get("changefreq", SiteMapBuilder.Namespace)).Should().Be(changeFrequencyElement);
        }

        [Fact]
        public void ThenNoLastChangedIsSpecified()
        {
            var sitemap = XDocument.Load(new StringReader(Result));

            var lastModifiedElement = new XElement(XName.Get("lastmod", SiteMapBuilder.Namespace), "2020-1-1");

            var urlElement = sitemap.Descendants(XName.Get("url", SiteMapBuilder.Namespace)).SingleOrDefault().Should().NotBeNull();
            _ = urlElement.And.Subject.Element(XName.Get("lastmod", SiteMapBuilder.Namespace)).Should().Be(lastModifiedElement);
        }

        [Fact]
        public void ThenTheUrlIsSpecified()
        {
            var sitemap = XDocument.Load(new StringReader(Result));

            var locationElement = new XElement(XName.Get("loc", SiteMapBuilder.Namespace), "/");

            var urlElement = sitemap.Descendants(XName.Get("url", SiteMapBuilder.Namespace)).SingleOrDefault().Should().NotBeNull();
            _ = urlElement.And.Subject.Element(XName.Get("loc", SiteMapBuilder.Namespace)).Should().Be(locationElement);
        }
    }
}
