using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Models;
using TheBookShop.Models.Repositories;
using TheBookShop.Models.ViewModels;
using Xunit;

namespace TheBookShop.Tests.AdminTests.ControllerTests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Product _product;

        public ProductControllerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();

            _productRepositoryMock.Setup(m => m.Products).Returns(new[]
            {
                new Product { ProductId = 1, Title = "Product1" },
                new Product { ProductId = 2, Title = "Product2" },
                new Product { ProductId = 3, Title = "Product3" },
            }.AsQueryable());

            _product = new Product { ProductId = 4, Title = "Product4" };
        }

        [Fact]
        public void Index_Contains_All_Products()
        {
            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object);
            var results = GetViewModel<ProductsListViewModel>(controller.Index());
            var products = results.Products.ToList();

            Assert.Equal(3, results.Products.Count());
            Assert.Equal("Product1", products[0].Title);
            Assert.Equal("Product2", products[1].Title);
            Assert.Equal("Product3", products[2].Title);
        }

        [Fact]
        public void Can_Edit_Product()
        {
            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object);
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
            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object);
            Product p1 = GetViewModel<Product>(controller.Edit(4));

            Assert.Null(p1);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object)
            {
                TempData = tempData.Object
            };
            
            IActionResult result = controller.Edit(_product, null);

            _productRepositoryMock.Verify(m => m.SaveProduct(_product));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult)?.ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object);
            controller.ModelState.AddModelError("error", "error");
           
            IActionResult result = controller.Edit(_product, null);

            _productRepositoryMock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Product()
        {
            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object);
            controller.Delete(_product.ProductId);

            _productRepositoryMock.Verify(m => m.DeleteProduct(_product.ProductId));
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}