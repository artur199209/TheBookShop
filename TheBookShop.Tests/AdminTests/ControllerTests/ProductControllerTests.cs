using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using TheBookShop.Models.ViewModels;
using Xunit;

namespace TheBookShop.Tests.AdminTests.ControllerTests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IProductCategoryRepository> _productCategoryRepositoryMock;
        private readonly Product _product;

        public ProductControllerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

            _productRepositoryMock.Setup(m => m.Products).Returns(new[]
            {
                new Product
                {
                    ProductId = 1, Title = "Product1", Opinions = new List<Opinion>
                    {
                        new Opinion { Name = "Name1", OpinionDescription = "OpinionDescription", Rating = 5 },
                        new Opinion { Name = "Name2", OpinionDescription = "OpinionDescription", Rating = 5 },
                        new Opinion { Name = "Name3", OpinionDescription = "OpinionDescription", Rating = 5 },
                    }
                },
                new Product { ProductId = 2, Title = "Product2" },
                new Product { ProductId = 3, Title = "Product3" },
            }.AsQueryable());

            _product = new Product { ProductId = 4, Title = "Product4" };
        }

        [Fact]
        public void Index_Contains_All_Products()
        {
            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object, _productCategoryRepositoryMock.Object);
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
            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object, _productCategoryRepositoryMock.Object);
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
            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object, _productCategoryRepositoryMock.Object);
            Product p1 = GetViewModel<Product>(controller.Edit(4));

            Assert.Null(p1);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object, _productCategoryRepositoryMock.Object)
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
            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object, _productCategoryRepositoryMock.Object);
            controller.ModelState.AddModelError("error", "error");
           
            IActionResult result = controller.Edit(_product, null);

            _productRepositoryMock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Product()
        {
            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object, _productCategoryRepositoryMock.Object);
            controller.Delete(_product.ProductId);

            _productRepositoryMock.Verify(m => m.DeleteProduct(_product.ProductId));
        }

        [Fact]
        public void Can_Paginate_Products()
        {
            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object, _productCategoryRepositoryMock.Object) { PageSize = 3 };

            ProductsListViewModel result = GetViewModel<ProductsListViewModel>(controller.Index());

            var products = result?.Products.ToArray();

            Assert.True(products?.Length == 3);
            Assert.Equal("Product1", products[0].Title);
            Assert.Equal("Product2", products[1].Title);
        }

        [Fact]
        public void Can_Send_Pagination_For_Products()
        {
            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object, _productCategoryRepositoryMock.Object) { PageSize = 3 };

            ProductsListViewModel result = GetViewModel<ProductsListViewModel>(controller.Index());

            PagingInfo pageInfo = result?.PagingInfo;

            Assert.Equal(3, pageInfo?.ItemsPerPage);
            Assert.Equal(3, pageInfo?.TotalItems);
            Assert.Equal(1, pageInfo?.TotalPages);
            Assert.Equal(1, pageInfo?.CurrentPage);
        }

        [Fact]
        public void Can_Display_Opinions()
        {
            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object, _productCategoryRepositoryMock.Object) { PageSize = 3 };
            OpinionsListViewModel result = GetViewModel<OpinionsListViewModel>(controller.Opinion(1));

            Assert.NotNull(result);
            Assert.NotNull(result.Opinions);
            Assert.Equal(3, result.Opinions.Count());
        }

        [Fact]
        public void Can_Paginate_Opinions()
        {
            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object, _productCategoryRepositoryMock.Object) { PageSize = 3 };
            OpinionsListViewModel result = GetViewModel<OpinionsListViewModel>(controller.Opinion(1));

            var opinions = result?.Opinions.ToArray();

            Assert.True(opinions?.Length == 3);
            Assert.Equal("Name1", opinions[0].Name);
            Assert.Equal("Name2", opinions[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_For_Opinions()
        {
            ProductController controller = new ProductController(_productRepositoryMock.Object, _authorRepositoryMock.Object, _productCategoryRepositoryMock.Object) { PageSize = 3 };

            OpinionsListViewModel result = GetViewModel<OpinionsListViewModel>(controller.Opinion(1));

            PagingInfo pageInfo = result?.PagingInfo;

            Assert.Equal(3, pageInfo?.ItemsPerPage);
            Assert.Equal(3, pageInfo?.TotalItems);
            Assert.Equal(1, pageInfo?.TotalPages);
            Assert.Equal(1, pageInfo?.CurrentPage);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}