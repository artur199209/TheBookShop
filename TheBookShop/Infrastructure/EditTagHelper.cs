using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TheBookShop.Infrastructure
{
    [HtmlTargetElement("edit", Attributes = "ram")]
    public class EditTagHelper : TagHelper
    {
        //[HtmlAttributeName("asp-for")]
        public ModelExpression AspFor { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var model = AspFor.ModelExplorer.Metadata.ContainerMetadata.ModelType.Name;
            var propName = AspFor.Metadata.Name;
            var id = $"{model}_{propName}";
            var name = $"{model}.{propName}";

            output.Attributes.Add("name", name);
            output.Attributes.Add("id", id);
            output.TagName = "input";
        }
    }
}