namespace FizzyLogic.Services
{
    using FizzyLogic.Models;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Extension methods for the URL helper
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Creates a URL for an article.
        /// </summary>
        /// <param name="url">The URL helper instance to use</param>
        /// <param name="article">The article instance to link</param>
        /// <returns>The absolute URL for the article</returns>
        public static string Article(this IUrlHelper url, Article article)
        {
#pragma warning disable IDE0050

            return url.Page("/Article", values: new
            {
                year = article.DatePublished.Value.Year,
                month = article.DatePublished.Value.Month,
                day = article.DatePublished.Value.Day,
                slug = article.Slug
            });

#pragma warning restore IDE0050
        }
    }
}

