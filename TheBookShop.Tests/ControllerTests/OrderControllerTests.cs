using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBookShop.Controllers;
using TheBookShop.Models;
using Xunit;

namespace TheBookShop.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_When_Empty_Cart()
        {
            var mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            Order order = new Order();
            OrderController target = new OrderController(mock.Object, cart);

            ViewResult result = target.Checkout(order) as ViewResult;

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_When_Shipping_Details_Invalid()
        {
            var mock = new Mock<IOrderRepository>();
            Product p1 = new Product{ ProductId = 1, Name = "Product1"};
            Cart cart = new Cart();
            cart.AddItem(p1, 1);
            
            OrderController target = new OrderController(mock.Object, cart);
            target.ModelState.AddModelError("error", "error");

            ViewResult result = target.Checkout(new Order()) as ViewResult;

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            var mock = new Mock<IOrderRepository>();
            Product p1 = new Product { ProductId = 1, Name = "Product1" };
            Cart cart = new Cart();
            cart.AddItem(p1, 1);

            OrderController target = new OrderController(mock.Object, cart);

            RedirectToActionResult result = target.Checkout(new Order()) as RedirectToActionResult;

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("Completed", result.ActionName);
        }
    }
}