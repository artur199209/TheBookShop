using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Models;
using Xunit;

namespace TheBookShop.Tests.ControllerTests
{
    public class BookControllerTests
    {
        [Fact]
        public void Index_Contains_All_Products()
        {
            Mock<IProductRepository> productRepoMock = new Mock<IProductRepository>();
            Mock<IAuthorRepository> authorRepoMock = new Mock<IAuthorRepository>();
            productRepoMock.Setup(m => m.Products).Returns((new []
            {
                new Product {ProductId = 1, Title = "Product1"},
                new Product {ProductId = 2, Title = "Product2"},
                new Product {ProductId = 3, Title = "Product3"},
            }).AsQueryable());

            BookController controller = new BookController(productRepoMock.Object, authorRepoMock.Object);
            Product[] results = GetViewModel<IEnumerable<Product>>(controller.Index())?.ToArray();

            Assert.Equal(3, results?.Length);
            Assert.Equal("Product1", results?[0].Title);
            Assert.Equal("Product2", results?[1].Title);
            Assert.Equal("Product3", results?[2].Title);
        }

        [Fact]
        public void Can_Edit_Product()
        {
            Mock<IProductRepository> productRepoMock = new Mock<IProductRepository>();
            Mock<IAuthorRepository> authorRepoMock = new Mock<IAuthorRepository>();
            productRepoMock.Setup(m => m.Products).Returns((new []
            {
                new Product {ProductId = 1, Title = "Product1"},
                new Product {ProductId = 2, Title = "Product2"},
                new Product {ProductId = 3, Title = "Product3"},
            }).AsQueryable());

            BookController controller = new BookController(productRepoMock.Object, authorRepoMock.Object);
            Product p1 = GetViewModel<Product>(controller.Edit(1));
            Product p2 = GetViewModel<Product>(controller.Edit(2));
            Product p3 = GetViewModel<Product>(controller.Edit(3));

            Assert.Equal(1, p1.ProductId);
            Assert.Equal(2, p2.ProductId);
            Assert.Equal(3, p3.ProductId);
        }

        [Fact]
        public void Cannot_Edit_Non_Existing_Product()
        {
            Mock<IProductRepository> productRepoMock = new Mock<IProductRepository>();
            Mock<IAuthorRepository> authorRepoMock = new Mock<IAuthorRepository>();
            productRepoMock.Setup(m => m.Products).Returns((new []
            {
                new Product {ProductId = 1, Title = "Product1"},
                new Product {ProductId = 2, Title = "Product2"},
                new Product {ProductId = 3, Title = "Product3"},
            }).AsQueryable());

            BookController controller = new BookController(productRepoMock.Object, authorRepoMock.Object);
            Product p1 = GetViewModel<Product>(controller.Edit(4));

            Assert.Null(p1);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            Mock<IProductRepository> productRepoMock = new Mock<IProductRepository>();
            Mock<IAuthorRepository> authorRepoMock = new Mock<IAuthorRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            BookController controller = new BookController(productRepoMock.Object, authorRepoMock.Object)
            {
                TempData = tempData.Object
            };
            
            Product product = new Product() { Title = "Test"};
            IActionResult result = controller.Edit(product, null);
            productRepoMock.Verify(m => m.SaveProduct(product));

            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult)?.ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            Mock<IProductRepository> productRepoMock = new Mock<IProductRepository>();
            Mock<IAuthorRepository> authorRepoMock = new Mock<IAuthorRepository>();

            BookController controller = new BookController(productRepoMock.Object, authorRepoMock.Object);
            controller.ModelState.AddModelError("error", "error");
            Product product = new Product() { Title = "Test" };

            IActionResult result = controller.Edit(product, null);

            productRepoMock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Product()
        {
            Product prod4 = new Product(){ProductId = 4, Title = "Product4"};

            Mock<IProductRepository> productRepoMock = new Mock<IProductRepository>();
            Mock<IAuthorRepository> authorRepoMock = new Mock<IAuthorRepository>();
            productRepoMock.Setup(m => m.Products).Returns((new []
            {
                new Product {ProductId = 1, Title = "Product1"},
                new Product {ProductId = 2, Title = "Product2"},
                new Product {ProductId = 3, Title = "Product3"},
            }).AsQueryable());

            BookController controller = new BookController(productRepoMock.Object, authorRepoMock.Object);
            controller.Delete(prod4.ProductId);

            productRepoMock.Verify(m => m.DeleteProduct(prod4.ProductId));
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}