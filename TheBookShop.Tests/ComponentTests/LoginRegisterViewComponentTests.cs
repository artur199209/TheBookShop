using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TheBookShop.Components;
using TheBookShop.Models;
using Xunit;

namespace TheBookShop.Tests.ComponentTests
{
    public class LoginRegisterViewComponentTests
    {
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly Mock<SignInManager<AppUser>> _mockSignInManager;

        public LoginRegisterViewComponentTests()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _mockSignInManager = new Mock<SignInManager<AppUser>>(_mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object, new Mock<ILogger<SignInManager<AppUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object);
        }

        [Fact]
        public void Correctly_Redirect_When_User_Is_Logged_In()
        {
            _mockSignInManager.Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);
            var loginRegisterViewCompponent = new LoginRegisterViewComponent(_mockSignInManager.Object, _mockUserManager.Object);

            var result = loginRegisterViewCompponent.InvokeAsync().Result;

            var viewName = (result as ViewViewComponentResult)?.ViewName;

            Assert.Equal("SignedIn", viewName);

        }

        [Fact]
        public void Correctly_Redirect_When_User_Is_Not_Logged_In()
        {
            _mockSignInManager.Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);
            var loginRegisterViewCompponent = new LoginRegisterViewComponent(_mockSignInManager.Object, _mockUserManager.Object);

            var result = loginRegisterViewCompponent.InvokeAsync().Result;

            var viewName = (result as ViewViewComponentResult)?.ViewName;

            Assert.Equal("SignedOut", viewName);

        }

        [Fact]
        public void View_Contains_User_Data_When_User_Is_Logged_In()
        {
            var appUser = new AppUser {UserName = "test"};

            _mockSignInManager.Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);
            _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(appUser);

            var loginRegisterViewCompponent = new LoginRegisterViewComponent(_mockSignInManager.Object, _mockUserManager.Object);

            var result = loginRegisterViewCompponent.InvokeAsync().Result;
            var userData = (result as ViewViewComponentResult)?.ViewData.Model as AppUser;

            Assert.NotNull(userData);
            Assert.Equal("test", userData.UserName);
        }
    }
}