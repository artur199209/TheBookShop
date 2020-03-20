using System.Linq;
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
    public class PaymentControllerTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;

        public PaymentControllerTests()
        {
            _paymentRepositoryMock = new Mock<IPaymentRepository>();

            _paymentRepositoryMock.Setup(x => x.Payments).Returns(new[]
            {
                new Payment { PaymentId = 1, Amount = 10 },
                new Payment { PaymentId = 2, Amount = 15 },
                new Payment { PaymentId = 3, Amount = 19 }
            }.AsQueryable());
        }

        [Fact]
        public void Index_Contains_All_Payments()
        {
            var paymentController = new PaymentController(_paymentRepositoryMock.Object);

            var result = CastHelper.GetViewModel<PaymentListViewModel>(paymentController.Index());
            var payments = result.Payments;
            Assert.NotNull(result);
            Assert.Equal(3, payments.Count());
        }

        [Fact]
        public void Can_Paginate_Orders()
        {
            var paymentController = new PaymentController(_paymentRepositoryMock.Object);
            var result = CastHelper.GetViewModel<PaymentListViewModel>(paymentController.Index());
            var payments = result?.Payments.ToArray();

            Assert.True(payments?.Length == 3);
            Assert.Equal(1, payments[0].PaymentId);
            Assert.Equal(2, payments[1].PaymentId);
            Assert.Equal(3, payments[2].PaymentId);
        }

        [Fact]
        public void Can_Send_Pagination_For_Orders()
        {
            var paymentController = new PaymentController(_paymentRepositoryMock.Object);
            var result = CastHelper.GetViewModel<PaymentListViewModel>(paymentController.Index());

            PagingInfo pageInfo = result?.PagingInfo;

            Assert.Equal(10, pageInfo?.ItemsPerPage);
            Assert.Equal(3, pageInfo?.TotalItems);
            Assert.Equal(1, pageInfo?.TotalPages);
            Assert.Equal(1, pageInfo?.CurrentPage);
        }
    }
}