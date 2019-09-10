using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using TheBookShop.Models;

namespace TheBookShop.Infrastructure
{
    [HtmlTargetElement("select", Attributes = "model-for")]
    public class SelectOptionTagHelper : TagHelper
    {
        private readonly IAuthorRepository _authorRepository;

        public SelectOptionTagHelper(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public ModelExpression ModelFor { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.AppendHtml((await output.GetChildContentAsync(false)).GetContent());

            string selected = ModelFor.Model as string;

            foreach (Author author in _authorRepository.Authors)
            {
                output.Content.AppendHtml(selected != null
                    ? $"<option selected value={author.AuthorId}>{author.Name} {author.Surname} </option>"
                    : $"<option value={author.AuthorId}>{author.Name} {author.Surname} </option>");
            }

            output.Attributes.SetAttribute("Name", ModelFor.Name);
            output.Attributes.SetAttribute("Id", ModelFor.Name);
        }
    }
}