// unset

namespace FizzyLogic.Markdown
{
    using Markdig;

    /// <summary>
    /// Extension methods to enable easy configuration of the markdown pipeline with website specific extensions.
    /// </summary>
    public static class FizzyLogicMarkdownExtensionMethods
    {
        /// <summary>
        /// Configures the website specific extensions for the markdown renderer.
        /// </summary>
        /// <param name="builder">Markdown pipeline builder to extend.</param>
        /// <returns>Returns the markdown pipeline builder instance.</returns>
        public static MarkdownPipelineBuilder UseFizzyLogicExtensions(this MarkdownPipelineBuilder builder)
        {
            builder.Extensions.Add(new FizzyLogicMarkdownExtension());
            return builder;
        }
    }
}