using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using TheBookShop.Tests.Helper;
using Xunit;

namespace TheBookShop.Tests.AdminTests.ControllerTests
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;

        public OrderControllerTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderRepositoryMock.Setup(x => x.Orders).Returns(new[]
            {
                new Order { OrderId = 1, Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Status = Order.OrderStatus.Shipped },
                new Order { OrderId = 2, Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Status = Order.OrderStatus.InProgress },
                new Order { OrderId = 3, Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Status = Order.OrderStatus.InProgress, },
                new Order { OrderId = 4, Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Status = Order.OrderStatus.Shipped },
                new Order { OrderId = 5, Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Status = Order.OrderStatus.Shipped }
            }.AsQueryable());
        }

        [Fact]
        public void Index_Contains_All_Orders()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);
            var result = CastHelper.GetViewModel<IEnumerable<Order>>(orderController.Index()).ToArray();

            Assert.NotNull(result);
            Assert.Equal(5, result.Length);
        }

        [Fact]
        public void Completed_Contains_Only_Completed_Orders()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);
            var result = CastHelper.GetViewModel<IEnumerable<Order>>(orderController.Completed()).ToArray();

            Assert.NotNull(result);
            Assert.Equal(3, result.Length);
        }

        [Fact]
        public void NotCompleted_Contains_Only_Not_Completed_Orders()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);
            var result = CastHelper.GetViewModel<IEnumerable<Order>>(orderController.NotCompleted()).ToArray();

            Assert.NotNull(result);
            Assert.Equal(2, result.Length);
        }

        [Fact]
        public void Can_Display_Existing_Order()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);

            var order1 = CastHelper.GetViewModel<Order>(orderController.Details(1));
            var order2 = CastHelper.GetViewModel<Order>(orderController.Details(2));
            var order3 = CastHelper.GetViewModel<Order>(orderController.Details(3));

            Assert.Equal(1, order1?.OrderId);
            Assert.Equal(2, order2?.OrderId);
            Assert.Equal(3, order3?.OrderId);
        }

        [Fact]
        public void Cannot_Display_Non_Existing_Order()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);

            var order = CastHelper.GetViewModel<Order>(orderController.Details(99));

            Assert.Null(order);
        }

        [Fact]
        public void Can_Add_Tracking_Number_To_Order()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);

            var result = orderController.AddTrackingNumber("123456", 1) as RedirectToActionResult;

            _orderRepositoryMock.Verify(m => m.SaveOrder(It.IsAny<Order>()));
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void Cannot_Add_Tracking_Number_When_Model_Is_Invalid()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);
            orderController.ModelState.AddModelError("error", "error");

            orderController.AddTrackingNumber("123456", 1);

            _orderRepositoryMock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never());
        }
    }
}