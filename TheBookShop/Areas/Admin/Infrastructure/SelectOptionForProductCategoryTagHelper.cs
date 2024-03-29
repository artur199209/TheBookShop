﻿using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using Serilog;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;

namespace TheBookShop.Areas.Admin.Infrastructure
{
    [HtmlTargetElement("select", Attributes = "model-for, ProductCategory")]
    public class SelectOptionForProductCategoryTagHelper : TagHelper
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public SelectOptionForProductCategoryTagHelper(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public ModelExpression ModelFor { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Log.Information($"Start processing {nameof(SelectOptionForProductCategoryTagHelper)}...");

            output.Content.AppendHtml((await output.GetChildContentAsync(false)).GetContent());

            var selected = ModelFor?.Model as ProductCategory;

            if (selected == null)
            {
                output.Content.AppendHtml($"<option disabled selected>Wybierz kategorię</option>");
            }

            foreach (var productCategory in _productCategoryRepository.ProductCategories)
            {
                output.Content.AppendHtml(selected?.ProductCategoryId != productCategory.ProductCategoryId
                    ? $"<option value={productCategory.ProductCategoryId}>{productCategory.Name} </option>"
                    : $"<option selected value={productCategory.ProductCategoryId}>{productCategory.Name} </option>");
            }

            output.Attributes.SetAttribute("Name", ModelFor?.Name);
            output.Attributes.SetAttribute("Id", ModelFor?.Name);

            Log.Information($"Finished processing {nameof(SelectOptionForProductCategoryTagHelper)}...");
        }
    }
}