using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Serilog;

namespace TheBookShop.Infrastructure
{
    [HtmlTargetElement("edit", Attributes = "ram")]
    public class EditTagHelper : TagHelper
    {
        public ModelExpression AspFor { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Log.Information($"Start processing {nameof(EditTagHelper)}...");
            var model = AspFor.ModelExplorer.Metadata.ContainerMetadata.ModelType.Name;
            var propName = AspFor.Metadata.Name;
            var id = $"{model}_{propName}";
            var name = $"{model}.{propName}";

            output.Attributes.Add("name", name);
            output.Attributes.Add("id", id);
            output.TagName = "input";
            Log.Information($"Finished processing {nameof(EditTagHelper)}...");
        }
    }
}