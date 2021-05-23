namespace FizzyLogic.Services
{
    using Markdig;
    using Markdown;

    /// <summary>
    /// Converter logic for markdown to text or HTML.
    /// </summary>
    public static class MarkdownConverter
    {
        /// <summary>
        /// Converts markdown to html using the standard fizzy logic markdown pipeline.
        /// </summary>
        /// <param name="markdown">Markdown to convert.</param>
        /// <returns>The converted HTML content.</returns>
        public static string ConvertToHtml(string markdown)
        {
            var markdownPipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseFizzyLogicExtensions()
                .Build();

            return Markdown.ToHtml(markdown, markdownPipeline);
        }
        
        /// <summary>
        /// Converts markdown to html using the standard fizzy logic markdown pipeline.
        /// </summary>
        /// <param name="markdown">Markdown to convert.</param>
        /// <returns>The converted HTML content.</returns>
        public static string ConvertToText(string markdown)
        {
            var markdownPipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseFizzyLogicExtensions()
                .Build();

            return Markdown.ToPlainText(markdown, markdownPipeline);
        }
    }
}