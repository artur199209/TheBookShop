using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using Serilog;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;

namespace TheBookShop.Infrastructure
{
    [HtmlTargetElement("select", Attributes = "model-for, Author")]
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
            Log.Information($"Start processing {nameof(SelectOptionTagHelper)}...");
            output.Content.AppendHtml((await output.GetChildContentAsync(false)).GetContent());

            var selected = ModelFor?.Model as Author;

            if (selected == null)
            {
                output.Content.AppendHtml($"<option disabled selected>Wybierz autora</option>");
            }

            foreach (var author in _authorRepository.Authors)
            {
                output.Content.AppendHtml(selected?.AuthorId != author.AuthorId
                    ? $"<option value={author.AuthorId}>{author.Name} {author.Surname} </option>"
                    : $"<option selected value={author.AuthorId}>{author.Name} {author.Surname} </option>");
            }

            output.Attributes.SetAttribute("Name", ModelFor?.Name);
            output.Attributes.SetAttribute("Id", ModelFor?.Name);
            Log.Information($"Start processing {nameof(SelectOptionTagHelper)}...");
        }
    }
}