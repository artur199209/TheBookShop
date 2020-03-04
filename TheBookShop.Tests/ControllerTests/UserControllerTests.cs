using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using TheBookShop.Controllers;
using TheBookShop.Models.DataModels;
using TheBookShop.Tests.Helper;
using Xunit;

namespace TheBookShop.Tests.ControllerTests
{
    public class UserControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
        private readonly Mock<IUserValidator<AppUser>> _userValidatorMock;
        private readonly Mock<IPasswordValidator<AppUser>> _passwordValidatorMock;
        private readonly Mock<IPasswordHasher<AppUser>> _passwordHasherMock;
        private const string Password = "user123";

        private readonly AppUser _appUser = new AppUser
        {
            UserName = "abcd",
            Id = "1234",
            Email = "wwww.ss@ww.pl"
        };

        public UserControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            _userManagerMock = new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _userValidatorMock = new Mock<IUserValidator<AppUser>>();
            _passwordValidatorMock = new Mock<IPasswordValidator<AppUser>>();
            _passwordHasherMock = new Mock<IPasswordHasher<AppUser>>();
            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
            _signInManagerMock = new Mock<SignInManager<AppUser>>(_userManagerMock.Object, contextAccessorMock.Object, userPrincipalFactoryMock.Object, null, null, null);
        }

        [Fact]
        public void Cannot_Login_When_Model_Is_Invalid()
        {
            var userController = new UserController(_userManagerMock.Object, _signInManagerMock.Object,
                _passwordValidatorMock.Object, _passwordHasherMock.Object);
            userController.ModelState.AddModelError("error", "error");

            var result = CastHelper.GetViewModel<LoginModel>(userController.Login(It.IsAny<LoginModel>()).Result);

            _signInManagerMock.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false), Times.Never);
            Assert.Null(result);
        }

        [Fact]
        public void Cannot_Login_When_User_Does_Not_Exist()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((AppUser)null);
            var userController = new UserController(_userManagerMock.Object, _signInManagerMock.Object,
                _passwordValidatorMock.Object, _passwordHasherMock.Object);
            var result = CastHelper.GetActionName(userController.Login(new LoginModel()).Result);
            _signInManagerMock.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false), Times.Never);

            Assert.Null(result);
        }

        [Fact]
        public void Can_Login()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new AppUser());
            _signInManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var userController = new UserController(_userManagerMock.Object, _signInManagerMock.Object,
                _passwordValidatorMock.Object, _passwordHasherMock.Object);

            var result = CastHelper.GetActionName(userController.Login(new LoginModel()).Result);

            _signInManagerMock.Verify(m => m.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), false, false));

            Assert.Equal("Index", result);
        }

        [Fact]
        public void Can_Change_Valid_Password()
        {
            _userManagerMock.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new AppUser());
            _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);

            _userValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Success);
            _passwordValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var userController = new UserController(_userManagerMock.Object, _signInManagerMock.Object, 
               _passwordValidatorMock.Object, _passwordHasherMock.Object);

            var result = CastHelper.GetActionName(userController.ChangePassword(_appUser.Id, _appUser.Email, Password).Result);
            _userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<AppUser>()));
            Assert.Equal("List", result);
        }
    }
}