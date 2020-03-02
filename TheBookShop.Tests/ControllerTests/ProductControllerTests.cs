using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBookShop.Controllers;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using TheBookShop.Models.ViewModels;
using Xunit;

namespace TheBookShop.Tests.ControllerTests
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;

        public ProductControllerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();

            _productRepositoryMock.Setup(m => m.Products).Returns(new []
            {
                new Product { ProductId = 1, Title = "Product1", IsProductInPromotion = true },
                new Product { ProductId = 2, Title = "Product2", IsProductInPromotion = true },
                new Product { ProductId = 3, Title = "Product3", IsProductInPromotion = true },
                new Product { ProductId = 4, Title = "Product4", IsProductInPromotion = true },
                new Product { ProductId = 5, Title = "Product5", IsProductInPromotion = true },
                new Product { ProductId = 6, Title = "Product6", IsProductInPromotion = false },
                new Product { ProductId = 7, Title = "Product7", IsProductInPromotion = false },
                new Product { ProductId = 8, Title = "Product8", IsProductInPromotion = false },
                new Product { ProductId = 9, Title = "Product9", IsProductInPromotion = false },
            }.AsQueryable());
        }

        [Fact]
        public void Can_Paginate()
        {
            var controller = new ProductController(_productRepositoryMock.Object) { PageSize = 3 };

            var result = GetViewModel<ProductsListViewModel>(controller.List(null, 2));

            var products = result?.Products.ToArray();
            
            Assert.True(products?.Length == 3);
            Assert.Equal("Product4", products[0].Title);
            Assert.Equal("Product5", products[1].Title);
            Assert.Equal("Product6", products[2].Title);
        }

        [Fact]
        public void Can_Send_Pagination()
        {
            var controller = new ProductController(_productRepositoryMock.Object) { PageSize = 3 };

            var result = GetViewModel<ProductsListViewModel>(controller.List(null, 2));

            var pageInfo = result?.PagingInfo;

            Assert.Equal(3, pageInfo?.ItemsPerPage);
            Assert.Equal(9, pageInfo?.TotalItems);
            Assert.Equal(3, pageInfo?.TotalPages);
            Assert.Equal(2, pageInfo?.CurrentPage);
        }

        [Fact]
        public void Index_Contains_Data_For_Carousel()
        {
            var productController = new ProductController(_productRepositoryMock.Object) { NewProductsCount = 3 };

            var result = GetViewModel<List<CarouselViewModel>>(productController.Index());

            var newProducts = result[0].Products.ToList();
            var promotionProducts = result[1].Products.ToList();
            var newProductsCategory = result[0].Category;
            var promotionProductsCategory = result[1].Category;

            Assert.NotNull(result);
            Assert.Equal(5, promotionProducts.Count);
            Assert.Equal(3, newProducts.Count);
            Assert.Equal("NOWOŚCI", newProductsCategory);
            Assert.Equal("PROMOCJE", promotionProductsCategory);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}