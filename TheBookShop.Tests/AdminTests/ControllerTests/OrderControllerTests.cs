using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Models;
using Xunit;

namespace TheBookShop.Tests.AdminTests.ControllerTests
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;

        public OrderControllerTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
        }

        [Fact]
        public void Index_Contains_All_Orders()
        {
            _mockOrderRepository.Setup(x => x.Orders).Returns(new[]
            {
                new Order {Customer = new Customer(), DeliveryAddress =  new DeliveryAddress()},
                new Order {Customer = new Customer(), DeliveryAddress =  new DeliveryAddress()},
                new Order {Customer = new Customer(), DeliveryAddress =  new DeliveryAddress()},
                new Order {Customer = new Customer(), DeliveryAddress =  new DeliveryAddress()}
            }.AsQueryable());

            var orderController = new OrderController(_mockOrderRepository.Object);

            var result = GetViewModel<IEnumerable<Order>>(orderController.Index()).ToArray();

            Assert.NotNull(result);
            Assert.Equal(4, result.Length);
        }

        [Fact]
        public void Completed_Contains_Only_Completed_Orders()
        {
            _mockOrderRepository.Setup(x => x.Orders).Returns(new[]
            {
                new Order {Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = true},
                new Order {Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = false},
                new Order {Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = false},
                new Order {Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = true},
                new Order {Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = true}
            }.AsQueryable());

            var orderController = new OrderController(_mockOrderRepository.Object);

            var result = GetViewModel<IEnumerable<Order>>(orderController.Completed()).ToArray();

            Assert.NotNull(result.Where(x => !x.Shipped));
            Assert.Equal(3, result.Length);
        }

        [Fact]
        public void NotCompleted_Contains_Only_Not_Completed_Orders()
        {
            _mockOrderRepository.Setup(x => x.Orders).Returns(new[]
            {
                new Order {Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = true},
                new Order {Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = false},
                new Order {Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = false},
                new Order {Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = true},
                new Order {Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Shipped = true}
            }.AsQueryable());

            var orderController = new OrderController(_mockOrderRepository.Object);

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