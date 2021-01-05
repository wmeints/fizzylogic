namespace FizzyLogic.Markdown
{
    using Markdig.Renderers;
    using Markdig.Renderers.Html;
    using Markdig.Renderers.Html.Inlines;
    using Markdig.Syntax.Inlines;

    /// <summary>
    /// Renders images with a figure and figcaption element instead of a plain img element.
    /// </summary>
    public class ImageRenderer : HtmlObjectRenderer<LinkInline>
    {
        private readonly LinkInlineRenderer _originalRenderer;

        /// <summary>
        /// Initializes a new instance of <see cref="ImageRenderer"/>.
        /// </summary>
        /// <param name="originalRenderer">The original image rendering logic.</param>
        public ImageRenderer(LinkInlineRenderer originalRenderer)
        {
            _originalRenderer = originalRenderer;
        }

        /// <summary>
        /// Writes the output for the provided inline link element.
        /// </summary>
        /// <param name="renderer">The HTML renderer to use.</param>
        /// <param name="obj">The link element to render.</param>
        protected override void Write(HtmlRenderer renderer, LinkInline obj)
        {
            if (obj.IsImage && renderer.EnableHtmlForInline)
            {
                _ = renderer.Write("<figure>");

                // The original renderer, will write the actual image tag for me.
                // So I'm not going to reimplement that.
                _originalRenderer.Write(renderer, obj);

                // Add a figure capture to the image tag.
                _ = renderer.Write("<figcaption>");

                // The inline stuff I grabbed from the original renderer.
                // This will render the label/title of the image for me.
                var wasHtmlEnabledForInline = renderer.EnableHtmlForInline;
                renderer.EnableHtmlForInline = false;

                renderer.WriteChildren(obj);

                renderer.EnableHtmlForInline = wasHtmlEnabledForInline;

                _ = renderer.Write("</figcaption>");
                _ = renderer.Write("</figure>");
            }
            else
            {
                _originalRenderer.Write(renderer, obj);
            }
        }
    }
}