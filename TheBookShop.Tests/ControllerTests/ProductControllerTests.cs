using System.Linq;
using Moq;
using TheBookShop.Controllers;
using TheBookShop.Infrastructure;
using TheBookShop.Models;
using TheBookShop.Models.ViewModel;
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
                new Product {ProductId = 1, Title = "Product1"},
                new Product {ProductId = 2, Title = "Product2"},
                new Product {ProductId = 3, Title = "Product3"},
                new Product {ProductId = 4, Title = "Product4"},
                new Product {ProductId = 5, Title = "Product5"}
            }.AsQueryable());
        }

        [Fact]
        public void Can_Paginate()
        {
            ProductController controller = new ProductController(_productRepositoryMock.Object) { PageSize = 3 };

            ProductsListViewModel result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            var products = result?.Products.ToArray();
            
            Assert.True(products?.Length == 2);
            Assert.Equal("Product4", products[0].Title);
            Assert.Equal("Product5", products[1].Title);
        }

        [Fact]
        public void Can_Send_Pagination()
        {
            ProductController controller = new ProductController(_productRepositoryMock.Object) { PageSize = 3 };

            ProductsListViewModel result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            PagingInfo pageInfo = result?.PagingInfo;

            Assert.Equal(3, pageInfo?.ItemsPerPage);
            Assert.Equal(5, pageInfo?.TotalItems);
            Assert.Equal(2, pageInfo?.TotalPages);
            Assert.Equal(2, pageInfo?.CurrentPage);
        }
    }
}