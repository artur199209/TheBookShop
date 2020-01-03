using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBookShop.Controllers;
using TheBookShop.Models;
using TheBookShop.Models.Repositories;
using TheBookShop.Models.ViewModels;
using Xunit;

namespace TheBookShop.Tests.ControllerTests
{
    public class SearchControllerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;

        public SearchControllerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _productRepositoryMock.Setup(m => m.Products).Returns(new[]
            {
                new Product { ProductId = 1, Title = "Product1" },
                new Product { ProductId = 2, Title = "Product2" },
                new Product { ProductId = 3, Title = "Product3" },
            }.AsQueryable());
        }

        [Fact]
        public void Search_Contains_All_Results()
        {
            var controller = new SearchController(_productRepositoryMock.Object);
            var results =  GetViewModel<ProductsListViewModel>(controller.SearchItems("Product")).Products.ToList();
            Assert.Equal(3, results.Count);
            Assert.Equal("Product1", results[0].Title);
            Assert.Equal("Product2", results[1].Title);
            Assert.Equal("Product3", results[2].Title);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}