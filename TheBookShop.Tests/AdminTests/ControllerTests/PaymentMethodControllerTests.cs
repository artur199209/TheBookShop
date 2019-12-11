using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using Xunit;

namespace TheBookShop.Tests.AdminTests.ControllerTests
{
    public class PaymentMethodControllerTests
    {
        private readonly Mock<IPaymentMethodRepository> _paymentMethodRepositoryMock;

        public PaymentMethodControllerTests()
        {
            _paymentMethodRepositoryMock = new Mock<IPaymentMethodRepository>();
        }

        [Fact]
        public void Index_Contains_All_Payment_Methods()
        {
            _paymentMethodRepositoryMock.Setup(x => x.PaymentMethods).Returns(new[]
            {
                new PaymentMethod(),
                new PaymentMethod(),
                new PaymentMethod()
            }.AsQueryable());

            var paymentMethodController = new PaymentMethodController(_paymentMethodRepositoryMock.Object);

            var result = GetViewModel<IEnumerable<PaymentMethod>>(paymentMethodController.Index());

            Assert.NotNull(result);
            Assert.Equal(_paymentMethodRepositoryMock.Object.PaymentMethods.Count(), result.Count());
        }

        [Fact]
        public void Can_Create_New_Delivery_Method()
        {
            var tempData = new Mock<ITempDataDictionary>();

            var deliveryMethodController =
                new PaymentMethodController(_paymentMethodRepositoryMock.Object)
                {
                    TempData = tempData.Object
                };

            deliveryMethodController.Create(It.IsAny<PaymentMethod>());
            _paymentMethodRepositoryMock.Verify(x => x.SavePaymentMethod(It.IsAny<PaymentMethod>()));
        }

        [Fact]
        public void Cannot_Create_New_Delivery_Method_When_Model_Is_Invalid()
        {
            var tempData = new Mock<ITempDataDictionary>();

            var deliveryMethodController =
                new PaymentMethodController(_paymentMethodRepositoryMock.Object)
                {
                    TempData = tempData.Object
                };

            deliveryMethodController.ModelState.AddModelError("error", "error");
            deliveryMethodController.Create(It.IsAny<PaymentMethod>());

            _paymentMethodRepositoryMock.Verify(x => x.SavePaymentMethod(It.IsAny<PaymentMethod>()), Times.Never());
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}