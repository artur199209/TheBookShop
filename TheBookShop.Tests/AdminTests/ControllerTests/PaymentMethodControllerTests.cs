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

            var paymentMethodController =
                new PaymentMethodController(_paymentMethodRepositoryMock.Object)
                {
                    TempData = tempData.Object
                };

            var result = paymentMethodController.Create(It.IsAny<PaymentMethod>());

            _paymentMethodRepositoryMock.Verify(x => x.SavePaymentMethod(It.IsAny<PaymentMethod>()));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult)?.ActionName);
        }

        [Fact]
        public void Cannot_Create_New_Delivery_Method_When_Model_Is_Invalid()
        {
            var tempData = new Mock<ITempDataDictionary>();

            var paymentMethodController =
                new PaymentMethodController(_paymentMethodRepositoryMock.Object)
                {
                    TempData = tempData.Object
                };

            paymentMethodController.ModelState.AddModelError("error", "error");
            var result = paymentMethodController.Create(It.IsAny<PaymentMethod>());

            _paymentMethodRepositoryMock.Verify(x => x.SavePaymentMethod(It.IsAny<PaymentMethod>()), Times.Never());
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Edit_Payment_Method()
        {
            _paymentMethodRepositoryMock.Setup(x => x.PaymentMethods).Returns(new[]
            {
                new PaymentMethod { PaymentMethodId = 1 },
                new PaymentMethod { PaymentMethodId = 2 },
                new PaymentMethod { PaymentMethodId = 3 }
            }.AsQueryable());

            var paymentMethodController =
                new PaymentMethodController(_paymentMethodRepositoryMock.Object);

            PaymentMethod p1 = GetViewModel<PaymentMethod>(paymentMethodController.Edit(1));
            PaymentMethod p2 = GetViewModel<PaymentMethod>(paymentMethodController.Edit(2));
            PaymentMethod p3 = GetViewModel<PaymentMethod>(paymentMethodController.Edit(3));

            Assert.Equal(1, p1.PaymentMethodId);
            Assert.Equal(2, p2.PaymentMethodId);
            Assert.Equal(3, p3.PaymentMethodId);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            var paymentMethodController =
                new PaymentMethodController(_paymentMethodRepositoryMock.Object);

            var result = paymentMethodController.Edit(It.IsAny<PaymentMethod>());

            _paymentMethodRepositoryMock.Verify(x => x.SavePaymentMethod(It.IsAny<PaymentMethod>()));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult)?.ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            var paymentMethodController =
                new PaymentMethodController(_paymentMethodRepositoryMock.Object);
            paymentMethodController.ModelState.AddModelError("error", "error");

            var result = paymentMethodController.Edit(new PaymentMethod());

            _paymentMethodRepositoryMock.Verify(x => x.SavePaymentMethod(It.IsAny<PaymentMethod>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}