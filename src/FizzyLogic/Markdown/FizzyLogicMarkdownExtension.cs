namespace FizzyLogic.Markdown
{
    using Markdig;
    using Markdig.Renderers;
    using Markdig.Renderers.Html.Inlines;
    using System.Linq;

    /// <summary>
    /// Markdown extension implementation for rendering website specific components.
    /// </summary>
    public class FizzyLogicMarkdownExtension : IMarkdownExtension
    {
        /// <summary>
        /// Configures the markdown pipeline.
        /// </summary>
        /// <param name="pipeline"></param>
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
        }

        /// <summary>
        /// Configures the markdown renderer.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <param name="renderer"></param>
        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            var originalRenderer = renderer.ObjectRenderers
                .First(x => typeof(LinkInlineRenderer) == x.GetType());

            _ = renderer.ObjectRenderers.Remove(originalRenderer);

            renderer.ObjectRenderers.Add(new ImageRenderer((LinkInlineRenderer)originalRenderer));
        }
    }
}