using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBookShop.Areas.Admin.Infrastructure;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using Xunit;

namespace TheBookShop.Tests.AdminTests.InfrastructureTests
{
    public class SelectOptionForProductCategoryTagHelperTests
    {
        private readonly Mock<IProductCategoryRepository> _productCategoryRepositoryMock;
        private readonly TagHelperOutput _output;
        private readonly TagHelperContext _ctx;

        public SelectOptionForProductCategoryTagHelperTests()
        {
            _productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

            var mockContent = new Mock<TagHelperContent>();

            _ctx = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(), "");

            _output = new TagHelperOutput("select",
                new TagHelperAttributeList(),
                (cache, encoder) => Task.FromResult(mockContent.Object));
        }

        [Fact]
        public async Task Output_Is_Null_When_There_Are_No_Product_Categories()
        {
            _productCategoryRepositoryMock.Setup(x => x.ProductCategories).Returns(new List<ProductCategory>().AsQueryable);

            var selectOptionForProductCategoryTagHelper = new SelectOptionForProductCategoryTagHelper(_productCategoryRepositoryMock.Object);
            await selectOptionForProductCategoryTagHelper.ProcessAsync(_ctx, _output);

            Assert.Equal("<option disabled selected>Wybierz kategorię</option>", _output.Content.GetContent());
        }

        [Fact]
        public async Task Output_Contains_Product_Categories()
        {
            _productCategoryRepositoryMock.Setup(x => x.ProductCategories).Returns(new[]
            {
                new ProductCategory { ProductCategoryId = 1, Name = "Name1" },
                new ProductCategory { ProductCategoryId = 2, Name = "Name2" }
            }.AsQueryable());

            var selectOptionForProductCategoryTagHelper = new SelectOptionForProductCategoryTagHelper(_productCategoryRepositoryMock.Object);
            await selectOptionForProductCategoryTagHelper.ProcessAsync(_ctx, _output);

            Assert.Equal("<option disabled selected>Wybierz kategorię</option><option value=1>Name1 </option><option value=2>Name2 </option>", _output.Content.GetContent());
        }
    }
}