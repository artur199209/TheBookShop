using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Areas.Admin.Model;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using TheBookShop.Models.ViewModels;
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
            var result = CastHelper.GetViewModel<OrderListViewModel>(orderController.Index());
            var orders = result.Orders.ToList();

            Assert.NotNull(result);
            Assert.Equal(5, orders.Count);
        }

        [Fact]
        public void Completed_Contains_Only_Completed_Orders()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);
            var result = CastHelper.GetViewModel<OrderListViewModel>(orderController.Completed());
            var orders = result.Orders.ToList();

            Assert.NotNull(result);
            Assert.Equal(3, orders.Count);
        }

        [Fact]
        public void NotCompleted_Contains_Only_Not_Completed_Orders()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);
            var result = CastHelper.GetViewModel<OrderListViewModel>(orderController.NotCompleted());
            var orders = result.Orders.ToList();

            Assert.NotNull(result);
            Assert.Equal(2, orders.Count);
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

        [Fact]
        public void Can_Paginate_Orders()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);
            var result = CastHelper.GetViewModel<OrderListViewModel>(orderController.Index());

            var orders = result?.Orders.ToArray();

            Assert.True(orders?.Length == 5);
            Assert.Equal(1, orders[0].OrderId);
            Assert.Equal(2, orders[1].OrderId);
            Assert.Equal(3, orders[2].OrderId);
            Assert.Equal(4, orders[3].OrderId);
            Assert.Equal(5, orders[4].OrderId);
        }

        [Fact]
        public void Can_Send_Pagination_For_Orders()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);
            var result = CastHelper.GetViewModel<OrderListViewModel>(orderController.Index());

            PagingInfo pageInfo = result?.PagingInfo;

            Assert.Equal(10, pageInfo?.ItemsPerPage);
            Assert.Equal(5, pageInfo?.TotalItems);
            Assert.Equal(1, pageInfo?.TotalPages);
            Assert.Equal(1, pageInfo?.CurrentPage);
        }
    }
}