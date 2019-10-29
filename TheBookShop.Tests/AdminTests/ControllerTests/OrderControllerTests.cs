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
                new Order { Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = true },
                new Order { Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = false },
                new Order { Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = false },
                new Order { Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = true },
                new Order { Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = true }
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

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

    }
}