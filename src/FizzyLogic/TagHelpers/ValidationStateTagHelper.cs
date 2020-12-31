namespace FizzyLogic.TagHelpers
{
    using System.Text.Encodings.Web;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.TagHelpers;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    /// <summary>
    /// Use this tag helper to show/hide the is-valid class on a validated field.
    /// </summary>

    [HtmlTargetElement("input", Attributes = "validation-state-for")]
    public class ValidationStateTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the  view context for the tag helper.
        /// </summary>
        /// <value></value>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Gets or sets the field to show the validation state for.
        /// </summary>
        /// <value></value>
        [HtmlAttributeName("validation-state-for")]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Processes the user input and hows/hide the is-invalid css class 
        /// on the element that this tag helper was used on.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext.ModelState.ContainsKey(For.Name))
            {
                if (ViewContext.ModelState[For.Name].ValidationState == ModelValidationState.Invalid)
                {
                    output.AddClass("is-invalid", HtmlEncoder.Default);
                }
            }

            base.Process(context, output);
        }
    }
}