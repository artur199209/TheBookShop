using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
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
                new Order { OrderId = 1, Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = true },
                new Order { OrderId = 2, Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = false },
                new Order { OrderId = 3, Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = false },
                new Order { OrderId = 4, Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = true },
                new Order { OrderId = 5, Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = true }
            }.AsQueryable());
        }

        [Fact]
        public void Index_Contains_All_Orders()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);
            var result = GetViewModel<IEnumerable<Order>>(orderController.Index()).ToArray();

            Assert.NotNull(result);
            Assert.Equal(5, result.Length);
        }

        [Fact]
        public void Completed_Contains_Only_Completed_Orders()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);
            var result = GetViewModel<IEnumerable<Order>>(orderController.Completed()).ToArray();

            Assert.NotNull(result.Where(x => !x.Shipped));
            Assert.Equal(3, result.Length);
        }

        [Fact]
        public void NotCompleted_Contains_Only_Not_Completed_Orders()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);
            var result = GetViewModel<IEnumerable<Order>>(orderController.NotCompleted()).ToArray();

            Assert.NotNull(result.Where(x => x.Shipped));
            Assert.Equal(2, result.Length);
        }

        [Fact]
        public void Can_Display_Existing_Order()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);

            var order1 = GetViewModel<Order>(orderController.Details(1));
            var order2 = GetViewModel<Order>(orderController.Details(2));
            var order3 = GetViewModel<Order>(orderController.Details(3));

            Assert.Equal(1, order1?.OrderId);
            Assert.Equal(2, order2?.OrderId);
            Assert.Equal(3, order3?.OrderId);
        }

        [Fact]
        public void Cannot_Display_Non_Existing_Order()
        {
            var orderController = new OrderController(_orderRepositoryMock.Object);

            var order = GetViewModel<Order>(orderController.Details(99));

            Assert.Null(order);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

    }
}