namespace FizzyLogic.TagHelpers
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.TagHelpers;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Models;
    using System.Threading.Tasks;

    /// <summary>
    /// Generates a permalink to an article.
    /// </summary>
    [HtmlTargetElement("permalink", Attributes="article")]
    public class PermalinkTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the article for which the link is generated.
        /// </summary>
        [HtmlAttributeName("article")]
        public Article Article { get; set; }

        /// <summary>
        /// Processes the tag content, converting the tag into a hyperlink to the specified article.
        /// </summary>
        /// <param name="context">Tag helper context to use.</param>
        /// <param name="output">Output produced for the tag helper.</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            
            var alternativeUrl =
                $"/{Article.DatePublished.Value.Year:0000}/" +
                $"{Article.DatePublished.Value.Month:00}/" +
                $"{Article.DatePublished.Value.Day:00}/" +
                $"{Article.Slug}";

            output.TagName = "a";
            output.Attributes.Add("href", alternativeUrl);
            output.Content.SetHtmlContent(childContent);
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}