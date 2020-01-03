using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBookShop.Controllers;
using TheBookShop.Models;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using Xunit;

namespace TheBookShop.Tests.ControllerTests
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Cart _cart;
        private readonly Product _product;

        public OrderControllerTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _cart = new Cart();
            _product = new Product { ProductId = 1, Title = "Product1" };

            _orderRepositoryMock.Setup(x => x.Orders).Returns(new[]
            {
                new Order { OrderId = 1, Customer = new Customer { Email = "email@email.com" }},
                new Order { OrderId = 2, Customer = new Customer { Email = "email@email.com" }},
                new Order { OrderId = 3, Customer = new Customer { Email = "email2@email.com" }}
            }.AsQueryable());
        }

        [Fact]
        public void Cannot_Checkout_When_Empty_Cart()
        {
            Order order = new Order();
            OrderController target = new OrderController(_orderRepositoryMock.Object, _cart);

            ViewResult result = target.Checkout(order) as ViewResult;

            _orderRepositoryMock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result?.ViewName));
            Assert.False(target.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_When_Shipping_Details_Invalid()
        {
            _cart.AddItem(_product, 1);
            
            OrderController target = new OrderController(_orderRepositoryMock.Object, _cart);
            target.ModelState.AddModelError("error", "error");

            ViewResult result = target.Checkout(new Order()) as ViewResult;

            _orderRepositoryMock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result?.ViewName));
            Assert.False(target.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            _cart.AddItem(_product, 1);

            OrderController target = new OrderController(_orderRepositoryMock.Object, _cart);

            RedirectToActionResult result = target.Checkout(new Order()) as RedirectToActionResult;

            _orderRepositoryMock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("Completed", result?.ActionName);
        }

        [Fact]
        public void MyOrders_Contains_All_Items()
        {
            var controller = new OrderController(_orderRepositoryMock.Object, _cart);

            var result = GetViewModel<IEnumerable<Order>>(controller.MyOrders("email@email.com"));

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void Can_Display_Order_Details()
        {
            var controller = new OrderController(_orderRepositoryMock.Object, _cart);

            var result = GetViewModel<Order>(controller.OrderDetails(1));

            Assert.NotNull(result);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}