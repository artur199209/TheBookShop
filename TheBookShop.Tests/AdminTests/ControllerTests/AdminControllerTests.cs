using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Models;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using TheBookShop.Tests.Helper;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TheBookShop.Tests.AdminTests.ControllerTests
{
    public class AdminControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;

        public AdminControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
            _userManagerMock = new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _signInManagerMock = new Mock<SignInManager<AppUser>>(_userManagerMock.Object, contextAccessorMock.Object, userPrincipalFactoryMock.Object, null, null, null);
            _orderRepositoryMock = new Mock<IOrderRepository>();

           PrepareOrderRepositoryMock();
        }

        [Fact]
        public void Cannot_Login_When_Model_Is_Invalid()
        {
            var controller = new AdminController(_userManagerMock.Object, _signInManagerMock.Object);
            controller.ModelState.AddModelError("error", "error");
            
            var result = CastHelper.GetViewModel<LoginModel>(controller.Login(It.IsAny<LoginModel>()).Result);

            _signInManagerMock.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false), Times.Never);
            Assert.Null(result);
        }

        [Fact]
        public void Cannot_Login_When_User_Does_Not_Exist()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((AppUser)null);
            var controller = new AdminController(_userManagerMock.Object, _signInManagerMock.Object);

            var result = CastHelper.GetActionName(controller.Login(new LoginModel()).Result);
            _signInManagerMock.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false), Times.Never);
            
            Assert.Null(result);
        }

        [Fact]
        public void Can_Login()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new AppUser());
            _signInManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), false, false))
                .ReturnsAsync(SignInResult.Success);

            var controller = new AdminController(_userManagerMock.Object, _signInManagerMock.Object);

            var result = CastHelper.GetActionName(controller.Login(new LoginModel()).Result);

            _signInManagerMock.Verify(m => m.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), false, false));

            Assert.Equal("Index", result);
        }

        [Fact]
        public void Index_Contains_Data_For_Charts()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new AppUser());
            _signInManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), false, false))
                .ReturnsAsync(SignInResult.Success);
            var controller = new AdminController(_userManagerMock.Object, _signInManagerMock.Object, _orderRepositoryMock.Object);

            var chartsData = CastHelper.GetViewModel<List<List<DataPoint>>>(controller.Index());
            
            Assert.NotNull(chartsData);
            Assert.Equal(2, chartsData.Count);
        }

        private void PrepareOrderRepositoryMock()
        {
            var category1 = new ProductCategory()
            {
                ProductCategoryId = 1,
                Name = "Category1"
            };

            var category2 = new ProductCategory()
            {
                ProductCategoryId = 2,
                Name = "Category2"
            };

            var product1 = new Product
            {
                ProductId = 1,
                Title = "Title1",
                Category = category1
            };

            var product2 = new Product
            {
                ProductId = 2,
                Title = "Title2",
                Category = category2
            };

            _orderRepositoryMock.Setup(x => x.Orders).Returns(new[]
            {
                new Order
                {
                    OrderId = 1, Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Status = Order.OrderStatus.Shipped, Lines = new List<CartLine>()
                    {
                        new CartLine
                        {
                            CartLineId = 1, Product = product1, Quantity = 1
                        },
                        new CartLine
                        {
                            CartLineId = 2, Product = product2, Quantity = 2
                        }
                    }
                },
                new Order { OrderId = 2, Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Status = Order.OrderStatus.InProgress },
                new Order { OrderId = 3, Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Status = Order.OrderStatus.InProgress, },
                new Order { OrderId = 4, Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Status = Order.OrderStatus.Shipped, Lines = new List<CartLine>()
                    {
                        new CartLine
                        {
                            CartLineId = 1, Product = product1, Quantity = 2
                        },
                        new CartLine
                        {
                            CartLineId = 2, Product = product2, Quantity = 1
                        }
                    }
                },
                new Order { OrderId = 5, Customer = new Customer(), DeliveryAddress =  new DeliveryAddress(), Status = Order.OrderStatus.Shipped, Lines = new List<CartLine>()
                    {
                        new CartLine
                        {
                            CartLineId = 1, Product = product1, Quantity = 1
                        },
                        new CartLine
                        {
                            CartLineId = 2, Product = product2, Quantity = 2
                        }
                    }
                }
            }.AsQueryable());
        }
    }
}