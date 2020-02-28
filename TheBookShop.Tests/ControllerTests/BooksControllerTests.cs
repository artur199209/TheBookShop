using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using TheBookShop.Controllers;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using TheBookShop.Models.ViewModels;
using Xunit;

namespace TheBookShop.Tests.ControllerTests
{
    public class BooksControllerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;

        public BooksControllerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();

            var product1 = new Product
            {
                ProductId = 1,
                SalesType = Product.SalesTypeEnums.Book
            };
            var product2 = new Product
            {
                ProductId = 2,
                SalesType = Product.SalesTypeEnums.BookSale
            };
            var product3 = new Product
            {
                ProductId = 3,
                SalesType = Product.SalesTypeEnums.BookSale
            };
            var product4 = new Product
            {
                ProductId = 4,
                SalesType = Product.SalesTypeEnums.BookPreview
            };
            var product5 = new Product
            {
                ProductId = 5,
                SalesType = Product.SalesTypeEnums.BookPreview
            };

            _productRepositoryMock.Setup(m => m.Products).Returns(new[]
            {
                product1, product2, product3, product4, product5
            }.AsQueryable());

            _orderRepositoryMock.Setup(m => m.Orders).Returns(new[]
            {
                new Order
                {
                    OrderId = 1, Status = Order.OrderStatus.Shipped, Lines = new List<CartLine>
                    {
                        new CartLine
                        {
                            Product = product1,
                            Quantity = 4
                        }
                    }
                },
                new Order
                {
                    OrderId = 2, Status = Order.OrderStatus.Shipped, Lines = new List<CartLine>
                    {
                        new CartLine
                        {
                            Product = product2,
                            Quantity = 3
                        }
                    }
                },
                new Order
                {
                    OrderId = 3, Status = Order.OrderStatus.Shipped, Lines = new List<CartLine>
                    {
                        new CartLine
                        {
                            Product = product2,
                            Quantity = 6
                        }
                    }
                },
                new Order
                {
                    OrderId = 4, Status = Order.OrderStatus.New, Lines = new List<CartLine>
                    {
                        new CartLine
                        {
                            Product = product3,
                            Quantity = 6
                        }
                    }
                },
                new Order
                {
                    OrderId = 5, Status = Order.OrderStatus.PreparedForShip, Lines = new List<CartLine>
                    {
                        new CartLine
                        {
                            Product = product4,
                            Quantity = 2
                        }
                    }
                },

            }.AsQueryable());
        }

        [Fact]
        public void Bestsellers_Contains_Only_Shipped_Items()
        {
            var controller = new BooksController(_orderRepositoryMock.Object, _productRepositoryMock.Object);

            var result = GetViewModel<IEnumerable<Product>>(controller.Bestsellers()).ToList();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Bestsellers_Return_Items_From_The_Largest_Quantity()
        {
            var controller = new BooksController(_orderRepositoryMock.Object, _productRepositoryMock.Object);

            var result = GetViewModel<IEnumerable<Product>>(controller.Bestsellers()).ToList();

            Assert.NotNull(result);
            Assert.Equal(2, result[0].ProductId);
            Assert.Equal(1, result[1].ProductId);
        }

        [Fact]
        public void Sales_Contains_Only_Products_With_Sales_Type()
        {
            var controller = new BooksController(_orderRepositoryMock.Object, _productRepositoryMock.Object);

            var result = GetViewModel<ProductsListViewModel>(controller.Sales()).Products.ToList();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Previews_Contains_Only_Products_With_Preview_Type()
        {
            var controller = new BooksController(_orderRepositoryMock.Object, _productRepositoryMock.Object);

            var result = GetViewModel<ProductsListViewModel>(controller.Previews()).Products.ToList();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Can_Paginate_Sales_Products()
        {
            var controller = new BooksController(_orderRepositoryMock.Object, _productRepositoryMock.Object);

            var result = GetViewModel<ProductsListViewModel>(controller.Sales()).PagingInfo;

            Assert.NotNull(result);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(4, result.ItemsPerPage);
            Assert.Equal(2, result.TotalItems);
        }

        [Fact]
        public void Can_Paginate_Previews_Products()
        {
            var controller = new BooksController(_orderRepositoryMock.Object, _productRepositoryMock.Object);

            var result = GetViewModel<ProductsListViewModel>(controller.Previews()).PagingInfo;

            Assert.NotNull(result);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(4, result.ItemsPerPage);
            Assert.Equal(2, result.TotalItems);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}