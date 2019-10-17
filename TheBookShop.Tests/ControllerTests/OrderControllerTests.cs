using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBookShop.Controllers;
using TheBookShop.Models;
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
        }

        [Fact]
        public void Cannot_Checkout_When_Empty_Cart()
        {
            Order order = new Order();
            OrderController target = new OrderController(_orderRepositoryMock.Object, _cart);

            ViewResult result = target.Checkout(order) as ViewResult;

            _orderRepositoryMock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result?.ViewName));
            Assert.False(result?.ViewData.ModelState.IsValid);
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
            Assert.False(result?.ViewData.ModelState.IsValid);
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
    }
}