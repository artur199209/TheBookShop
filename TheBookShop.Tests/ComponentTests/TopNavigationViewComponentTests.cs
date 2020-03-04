using Moq;
using System.Collections.Generic;
using System.Linq;
using TheBookShop.Components;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using TheBookShop.Tests.Helper;
using Xunit;

namespace TheBookShop.Tests.ComponentTests
{
    public class TopNavigationViewComponentTests
    {
        private readonly TopNavigationViewComponent _topNavigationViewComponent;

        public TopNavigationViewComponentTests()
        {
            var productCategoryRepository = new Mock<IProductCategoryRepository>();

            productCategoryRepository.Setup(m => m.ProductCategories).Returns(new[]
            {
                new ProductCategory { ProductCategoryId = 1 , Name = "Category1" },
                new ProductCategory { ProductCategoryId = 2 , Name = "Category2" },
                new ProductCategory { ProductCategoryId = 3 , Name = "Category3" },
            }.AsQueryable());

            _topNavigationViewComponent = new TopNavigationViewComponent(productCategoryRepository.Object);
        }

        [Fact]
        public void Top_Navigation_Panel_Is_Not_Null()
        {
            var results = CastHelper.GetViewComponentModel<IEnumerable<NavbarItem>>(_topNavigationViewComponent.Invoke()).ToArray();

            Assert.NotNull(_topNavigationViewComponent);
            Assert.NotNull(results);
        }

        [Fact]
        public void Top_Navigation_Panel_Contains_Parents()
        {
            var results = CastHelper.GetViewComponentModel<IEnumerable<NavbarItem>>(_topNavigationViewComponent.Invoke()).ToArray();
            var parents = results.Where(x => x.IsParent);

            Assert.NotNull(parents);
        }

        [Fact]
        public void Top_Navigation_Panel_Contains_Childrens()
        {
            var results = CastHelper.GetViewComponentModel<IEnumerable<NavbarItem>>(_topNavigationViewComponent.Invoke()).ToArray();
            var childrens = results.Where(x => x.IsParent);

            Assert.NotNull(childrens);
        }
    }
}