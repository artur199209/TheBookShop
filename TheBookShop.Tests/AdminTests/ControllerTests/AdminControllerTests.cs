using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Models.DataModels;
using TheBookShop.Tests.Helper;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TheBookShop.Tests.AdminTests.ControllerTests
{
    public class AdminControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<SignInManager<AppUser>> _signInManagerMock;

        public AdminControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
            _userManagerMock = new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _signInManagerMock = new Mock<SignInManager<AppUser>>(_userManagerMock.Object, contextAccessorMock.Object, userPrincipalFactoryMock.Object, null, null, null);
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
    }
}