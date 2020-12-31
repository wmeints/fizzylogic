namespace FizzyLogic.Models
{
    using System;

    /// <summary>
    /// A blogpost is modeled as an article.
    /// </summary>
    public class Article
    {
        /// <summary>
        /// Gets or sets the ID of the article.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the slug for the article. 
        /// This is used to identify the article in the URL.
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets header image for the article. 
        /// This is used on tiles and in the header of the article.
        /// </summary>
        public string FeaturedImage { get; set; }

        /// <summary>
        /// Gets or sets the title of the article.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the excerpt of the article. 
        /// This is used in the RSS feed and on tiles.
        /// </summary>
        public string Excerpt { get; set; }

        /// <summary>
        /// Gets or sets the markdown source for the article.
        /// </summary>
        public string Markdown { get; set; }

        /// <summary>
        /// Gets or sets the mobiledoc source for the aritlce.
        /// This is only used in migration scenarios.
        /// </summary>
        public string MobileDoc { get; set; }

        /// <summary>
        /// Gets or sets the rendered HTML content.
        /// This is used to render the article content on the website.
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// Gets or sets the date the article is published.
        /// Setting this to null turns the article into a draft article.
        /// </summary>
        public DateTime? DatePublished { get; set; }

        /// <summary>
        /// Gets or sets the date the article was created.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the date the article was modified.
        /// </summary>
        public DateTime DateModified { get; set; }

        /// <summary>
        /// Gets or sets the author for the article.
        /// </summary>
        public ApplicationUser Author { get; set; }

        /// <summary>
        /// Gets or sets the category in which the article was filed.
        /// </summary>
        public Category Category { get; set; }
    }
}