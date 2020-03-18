using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBookShop.Controllers;
using TheBookShop.Models.DataModels;
using TheBookShop.Tests.Helper;
using Xunit;

namespace TheBookShop.Tests.ControllerTests
{
    public class RegisterControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;

        public RegisterControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            _userManagerMock = new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public void Can_Create_New_User()
        {
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            var registerController = new RegisterController(_userManagerMock.Object);

            var viewName = CastHelper.GetViewName(registerController.Register(new CreateModel()).Result);
            _userManagerMock.Verify(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()));
            
            Assert.Equal("Info", viewName);
        }

        [Fact]
        public void Cannot_Create_User_When_Model_Is_Invalid()
        {
            var registerController = new RegisterController(_userManagerMock.Object);

            registerController.ModelState.AddModelError("error", "error");
            var result = registerController.Register(null).Result;
            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<AppUser>()), Times.Never());
            Assert.IsType<ViewResult>(result);
        }
    }
}