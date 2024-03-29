﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBookShop.Controllers;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using TheBookShop.Tests.Helper;
using Xunit;

namespace TheBookShop.Tests.ControllerTests
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IPaymentMethodRepository> _paymentMethodRepositoryMock;
        private readonly Cart _cart;
        private readonly Product _product;

        public OrderControllerTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _paymentMethodRepositoryMock = new Mock<IPaymentMethodRepository>();
            _cart = new Cart();
            _product = new Product { ProductId = 1, Title = "Product1" };

            _orderRepositoryMock.Setup(x => x.Orders).Returns(new[]
            {
                new Order
                {
                    OrderId = 1, Customer = new Customer { Email = "email@email.com" },
                    Lines = new List<CartLine>()
                    {
                        new CartLine
                        {
                            CartLineId = 1, Product = new Product { IsProductInPromotion = true, Price = 20, PromotionalPrice = 10 }, Quantity = 4
                        },
                        new CartLine
                        {
                            CartLineId = 2, Product = new Product { IsProductInPromotion = false, Price = 15, PromotionalPrice = 10 }, Quantity = 2
                        }
                    },
                    DeliveryPaymentMethod = new DeliveryPaymentMethod
                    {
                        PaymentMethod = new PaymentMethod { Price = 0 }
                    }
                },
                new Order { OrderId = 2, Customer = new Customer { Email = "email@email.com" }},
                new Order { OrderId = 3, Customer = new Customer { Email = "email2@email.com" }}
            }.AsQueryable());

            _paymentMethodRepositoryMock.Setup(x => x.PaymentMethods).Returns(new[]
            {
                new PaymentMethod() { Name = "Test", PaymentMethodId = 1, Price = 10 }
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

            OrderController target = new OrderController(_orderRepositoryMock.Object, _cart, _paymentMethodRepositoryMock.Object);

            RedirectToActionResult result = target.Checkout(new Order
            {
                DeliveryPaymentMethod = new DeliveryPaymentMethod()
                {
                    PaymentMethodId = 1
                },
                
            }) as RedirectToActionResult;

            _orderRepositoryMock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("Completed", result?.ActionName);
        }

        [Fact]
        public void MyOrders_Contains_All_Items()
        {
            var controller = new OrderController(_orderRepositoryMock.Object, _cart);

            var result = CastHelper.GetViewModel<IEnumerable<Order>>(controller.MyOrders("email@email.com"));

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void Can_Display_Order_Details()
        {
            var controller = new OrderController(_orderRepositoryMock.Object, _cart);

            var result = CastHelper.GetViewModel<Order>(controller.OrderDetails(1));

            Assert.NotNull(result);
        }

        [Fact]
        public void Can_Calculate_Total_Costs()
        {
            var controller = new OrderController(_orderRepositoryMock.Object, _cart);

            var orderDetails = CastHelper.GetViewModel<Order>(controller.OrderDetails(1));

            var totalCosts = orderDetails.CalculateTotalCosts();

            Assert.Equal(70, totalCosts);
        }
    }
}