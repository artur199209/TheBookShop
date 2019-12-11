using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Areas.Admin.Model;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using Xunit;

namespace TheBookShop.Tests.AdminTests.ControllerTests
{
    public class DeliveryMethodControllerTests
    {
        private readonly Mock<IDeliveryMethodRepository> _deliveryMethodRepositoryMock;
        private readonly Mock<IPaymentMethodRepository> _paymentMethodRepository;

        public DeliveryMethodControllerTests()
        {
            _deliveryMethodRepositoryMock = new Mock<IDeliveryMethodRepository>();
            _paymentMethodRepository = new Mock<IPaymentMethodRepository>();
        }

        [Fact]
        public void Index_Contains_All_Delivery_Methods()
        {
            _deliveryMethodRepositoryMock.Setup(x => x.DeliveryMethods).Returns(new[]
            {
                new DeliveryMethod(),
                new DeliveryMethod(),
                new DeliveryMethod(),
                new DeliveryMethod()
            }.AsQueryable());

            var deliveryMethodController =
                new DeliveryMethodController(_deliveryMethodRepositoryMock.Object, _paymentMethodRepository.Object);

            var result = GetViewModel<IEnumerable<DeliveryMethod>>(deliveryMethodController.Index());

            Assert.NotNull(result);
            Assert.Equal(_deliveryMethodRepositoryMock.Object.DeliveryMethods.Count(), result.Count());
        }

        [Fact]
        public void Can_Create_New_Delivery_Method()
        {
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            var deliveryMethodController =
                new DeliveryMethodController(_deliveryMethodRepositoryMock.Object, _paymentMethodRepository.Object)
                {
                    TempData = tempData.Object
                };

            deliveryMethodController.Create(It.IsAny<string>());
            _deliveryMethodRepositoryMock.Verify(x => x.SaveDeliveryMethod(It.IsAny<DeliveryMethod>()));
        }

        [Fact]
        public void Cannot_Create_New_Delivery_Method_When_Model_Is_Invalid()
        {
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            var deliveryMethodController =
                new DeliveryMethodController(_deliveryMethodRepositoryMock.Object, _paymentMethodRepository.Object)
                {
                    TempData = tempData.Object
                };

            deliveryMethodController.ModelState.AddModelError("error", "error");
            deliveryMethodController.Create(It.IsAny<string>());

            _deliveryMethodRepositoryMock.Verify(x => x.SaveDeliveryMethod(It.IsAny<DeliveryMethod>()), Times.Never());
        }
        
        [Fact]
        public void Can_Edit_Delivery_Method()
        {
            SetUpMocks();

            var deliveryMethodController =
                new DeliveryMethodController(_deliveryMethodRepositoryMock.Object, _paymentMethodRepository.Object);

            var result = GetViewModel<DeliveryPaymentViewModel>(deliveryMethodController.Edit(1));

            Assert.NotNull(result);
            Assert.Equal(2, result.NonPaymentMethods.Count);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

        private string GetActionName(IActionResult result)
        {
            return (result as RedirectToActionResult)?.ActionName;
        }

        private void SetUpMocks()
        {
            _deliveryMethodRepositoryMock.Setup(x => x.DeliveryMethods).Returns(new[]
            {
                new DeliveryMethod
                {
                    DeliveryMethodId = 1,
                    PaymentMethods = new List<DeliveryPaymentMethod>
                    {
                        new DeliveryPaymentMethod
                        {
                            PaymentMethodId = 1,
                            DeliveryMethodId = 1,
                            PaymentMethod = new PaymentMethod()
                        }
                    }
                },
                new DeliveryMethod
                {
                    DeliveryMethodId = 2,
                    PaymentMethods = new List<DeliveryPaymentMethod>
                    {
                        new DeliveryPaymentMethod
                        {
                            PaymentMethodId = 2,
                            DeliveryMethodId = 1,
                            PaymentMethod = new PaymentMethod()
                        }
                    }
                },
                new DeliveryMethod
                {
                    DeliveryMethodId = 3,
                    PaymentMethods = new List<DeliveryPaymentMethod>
                    {
                        new DeliveryPaymentMethod
                        {
                            PaymentMethodId = 3,
                            DeliveryMethodId = 1,
                            PaymentMethod = new PaymentMethod()
                        }
                    }
                }
            }.AsQueryable);

            _paymentMethodRepository.Setup(x => x.PaymentMethods).Returns(new[]
            {
                new PaymentMethod { PaymentMethodId = 1 },
                new PaymentMethod { PaymentMethodId = 2 },
                new PaymentMethod { PaymentMethodId = 3 }
            }.AsQueryable);
        }
    }
}