using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using TheBookShop.Components;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using Xunit;

namespace TheBookShop.Tests.ComponentTests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new []
            {
                new Product { ProductId = 1, Title = "Product1", Category = new ProductCategory { Name = "Category1" }},
                new Product { ProductId = 2, Title = "Product2", Category = new ProductCategory { Name = "Category2" }},
                new Product { ProductId = 3, Title = "Product3", Category = new ProductCategory { Name = "Category1" }},
                new Product { ProductId = 4, Title = "Product4", Category = new ProductCategory { Name = "Category3" }},
                new Product { ProductId = 5, Title = "Product5", Category = new ProductCategory { Name = "Category1" }}
            }.AsQueryable());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            var results = GetViewModel<IEnumerable<string>>(target.Invoke()).ToArray();

            Assert.True(new []{"Category1", "Category2", "Category3"}.SequenceEqual(results));
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            string categoryToSelect = "Category3";

            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new []
            {
                new Product { ProductId = 1, Title = "Product1", Category = new ProductCategory { Name = "Category1" }},
                new Product { ProductId = 2, Title = "Product2", Category = new ProductCategory { Name = "Category2" }},
                new Product { ProductId = 3, Title = "Product3", Category = new ProductCategory { Name = "Category1" }},
                new Product { ProductId = 4, Title = "Product4", Category = new ProductCategory { Name = "Category3" }},
                new Product { ProductId = 5, Title = "Product5", Category = new ProductCategory { Name = "Category1" }}
            }.AsQueryable());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object)
            {
                ViewComponentContext = new ViewComponentContext
                {
                    ViewContext = new ViewContext
                    {
                        RouteData = new RouteData()
                    }
                }
            };

            target.RouteData.Values["category"] = categoryToSelect;

            var result = (string)(target.Invoke() as ViewViewComponentResult)?.ViewData["SelectedCategory"];

            Assert.Equal(categoryToSelect, result);
        }

        private T GetViewModel<T>(IViewComponentResult result) where T : class
        {
            return (result as ViewViewComponentResult)?.ViewData.Model as T;
        }
    }
}