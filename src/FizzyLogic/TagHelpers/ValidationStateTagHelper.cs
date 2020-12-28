using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FizzyLogic.TagHelpers
{
    [HtmlTargetElement("input", Attributes = "validation-state-for")]
    public class ValidationStateTagHelper : TagHelper
    {
        [ViewContext] [HtmlAttributeNotBound] public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("validation-state-for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext.ModelState.ContainsKey(For.Name))
            {
                if (ViewContext.ModelState[For.Name].ValidationState == ModelValidationState.Invalid)
                {
                    output.AddClass("is-invalid",HtmlEncoder.Default);
                }
            }

            base.Process(context, output);
        }
    }
}