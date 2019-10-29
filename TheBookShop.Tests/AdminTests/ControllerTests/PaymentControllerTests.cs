using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Models;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using Xunit;

namespace TheBookShop.Tests.AdminTests.ControllerTests
{
    public class PaymentControllerTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;

        public PaymentControllerTests()
        {
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
        }

        [Fact]
        public void Index_Contains_All_Payments()
        {
            _paymentRepositoryMock.Setup(x => x.Payments).Returns(new[]
            {
                new Payment { PaymentId = 1, Amount = 10 },
                new Payment { PaymentId = 2, Amount = 15 },
                new Payment { PaymentId = 3, Amount = 19 },
            }.AsQueryable());

            var paymentController = new PaymentController(_paymentRepositoryMock.Object);

            var result = GetViewModel<IEnumerable<Payment>>(paymentController.Index()).ToArray();

            Assert.NotNull(result);
            Assert.Equal(3, result.Length);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}