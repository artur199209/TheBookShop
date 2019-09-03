using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using TheBookShop.Components;
using TheBookShop.Models;
using Xunit;

namespace TheBookShop.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Title = "Product1", Category = "Category1"},
                new Product {ProductId = 2, Title = "Product2", Category = "Category2"},
                new Product {ProductId = 3, Title = "Product3", Category = "Category1"},
                new Product {ProductId = 4, Title = "Product4", Category = "Category3"},
                new Product {ProductId = 5, Title = "Product5", Category = "Category1"}
            }).AsQueryable());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            var results = ((IEnumerable<string>)(target.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();

            Assert.True(Enumerable.SequenceEqual(new string[]{"Category1", "Category2", "Category3"}, results));
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            string categoryToSelect = "Category3";

            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Title = "Product1", Category = "Category1"},
                new Product {ProductId = 2, Title = "Product2", Category = "Category2"},
                new Product {ProductId = 3, Title = "Product3", Category = "Category1"},
                new Product {ProductId = 4, Title = "Product4", Category = "Category3"},
                new Product {ProductId = 5, Title = "Product5", Category = "Category1"}
            }).AsQueryable());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new RouteData()
                }
            };

            target.RouteData.Values["category"] = categoryToSelect;

            var result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            Assert.Equal(categoryToSelect, result);
        }
    }
}