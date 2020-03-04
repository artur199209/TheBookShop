using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Models.DataModels;
using TheBookShop.Tests.Helper;
using Xunit;

namespace TheBookShop.Tests.AdminTests.ControllerTests
{
    public class AccountControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<IUserValidator<AppUser>> _userValidatorMock;
        private readonly Mock<IPasswordValidator<AppUser>> _passwordValidatorMock;
        private readonly Mock<IPasswordHasher<AppUser>> _passwordHasherMock;
        private const string Password = "admin123";

        private readonly AppUser _appUser = new AppUser
        {
            UserName = "abcd",
            Id = "1234",
            Email = "wwww.ss@ww.pl"
        };

        public AccountControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            _userManagerMock = new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _userValidatorMock = new Mock<IUserValidator<AppUser>>();
            _passwordValidatorMock = new Mock<IPasswordValidator<AppUser>>();
            _passwordHasherMock = new Mock<IPasswordHasher<AppUser>>();
        }

        [Fact]
        public void Index_Contains_All_Users()
        {
            _userManagerMock.Setup(x => x.Users).Returns(new[]
            {
                new AppUser(),
                new AppUser(),
                new AppUser(),
                new AppUser()
            }.AsQueryable());

            var accountController = new AccountController(_userManagerMock.Object, _userValidatorMock.Object, _passwordValidatorMock.Object, _passwordHasherMock.Object);
            
            var result = CastHelper.GetViewModel<IEnumerable<AppUser>>(accountController.Index());

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void Can_Edit_User()
        {
            _userManagerMock.Setup(userManager => userManager.FindByIdAsync("1"))
                .ReturnsAsync(new AppUser { Id = "1"});
            _userManagerMock.Setup(userManager => userManager.FindByIdAsync("2"))
                .ReturnsAsync(new AppUser { Id = "2"});
            _userManagerMock.Setup(userManager => userManager.FindByIdAsync("3"))
                .ReturnsAsync(new AppUser { Id = "3"});

            var accountController = new AccountController(_userManagerMock.Object, _userValidatorMock.Object, _passwordValidatorMock.Object, _passwordHasherMock.Object);

            var user1 = CastHelper.GetViewModel<AppUser>(accountController.Edit("1").Result);
            var user2 = CastHelper.GetViewModel<AppUser>(accountController.Edit("2").Result);
            var user3 = CastHelper.GetViewModel<AppUser>(accountController.Edit("3").Result);

            Assert.Equal("1", user1?.Id);
            Assert.Equal("2", user2?.Id);
            Assert.Equal("3", user3?.Id);
        }

        [Fact]
        public void Cannot_Edit_Non_Existing_User()
        {
            _userManagerMock.Setup(userManager => userManager.FindByIdAsync("1"))
                .ReturnsAsync(new AppUser());
            _userManagerMock.Setup(userManager => userManager.FindByIdAsync("2"))
                .ReturnsAsync(new AppUser());

            var accountController = new AccountController(_userManagerMock.Object, _userValidatorMock.Object, _passwordValidatorMock.Object, _passwordHasherMock.Object);

            var result = CastHelper.GetViewModel<AppUser>(accountController.Edit("3").Result);

            Assert.Null(result);
        }

        [Fact]
        public void Can_Create_New_User()
        {
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            var accountController = new AccountController(_userManagerMock.Object, _userValidatorMock.Object, _passwordValidatorMock.Object, _passwordHasherMock.Object);
            
            var result = CastHelper.GetActionName(accountController.Create(new CreateModel()).Result);
            _userManagerMock.Verify(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()));
            Assert.Equal("Index", result);
        }

        [Fact]
        public void Cannot_Create_User_When_Model_Is_Invalid()
        {
            var accountController = new AccountController(_userManagerMock.Object, _userValidatorMock.Object, _passwordValidatorMock.Object, _passwordHasherMock.Object);

            accountController.ModelState.AddModelError("error", "error");
            var result = accountController.Create(null).Result;
            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<AppUser>()), Times.Never());
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
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
            
            var accountController = new AccountController(_userManagerMock.Object,
                _userValidatorMock.Object,
                _passwordValidatorMock.Object, _passwordHasherMock.Object);
            
            var result = CastHelper.GetActionName(accountController.Edit(_appUser.Id, _appUser.Email, Password).Result);
            _userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<AppUser>()));
            Assert.Equal("Index", result);
        }

        [Fact]
        public void Cannot_Save_Invalid_Data()
        {
            _userManagerMock.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new AppUser());
            _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Failed());

            _userValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Failed());
            _passwordValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var accountController = new AccountController(_userManagerMock.Object,
                _userValidatorMock.Object,
                _passwordValidatorMock.Object, _passwordHasherMock.Object);

            accountController.ModelState.AddModelError("error", "error");
            
            var result = accountController.Edit(_appUser.Id, _appUser.Email, Password).Result;

            _userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<AppUser>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Cannot_Update_When_Password_Is_Invalid()
        {
            _userManagerMock.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new AppUser());
            _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Failed());

            _userValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Success);
            _passwordValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var accountController = new AccountController(_userManagerMock.Object,
                _userValidatorMock.Object,
                _passwordValidatorMock.Object, _passwordHasherMock.Object);

            accountController.ModelState.AddModelError("error", "error");

            var result = accountController.Edit(_appUser.Id, _appUser.Email, Password).Result;

            _userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<AppUser>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Cannot_Update_When_Username_Is_Invalid()
        {
            _userManagerMock.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new AppUser());
            _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Failed());

            _userValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Failed());
            _passwordValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var accountController = new AccountController(_userManagerMock.Object,
                _userValidatorMock.Object,
                _passwordValidatorMock.Object, _passwordHasherMock.Object);

            accountController.ModelState.AddModelError("error", "error");

            var result = accountController.Edit(_appUser.Id, _appUser.Email, Password).Result;

            _userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<AppUser>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Cannot_Update_Non_Existing_User()
        {
            var accountController = new AccountController(_userManagerMock.Object,
               _userValidatorMock.Object,
                _passwordValidatorMock.Object, _passwordHasherMock.Object);
            
            var result = (accountController.Edit(null, null, null).Result);
            _userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<AppUser>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Existing_User()
        {
            _userManagerMock.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new AppUser());
            _userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            var accountController = new AccountController(_userManagerMock.Object, _userValidatorMock.Object, _passwordValidatorMock.Object, _passwordHasherMock.Object);

            var result = CastHelper.GetActionName(accountController.Delete("1234").Result);
            _userManagerMock.Verify(m => m.DeleteAsync(It.IsAny<AppUser>()));
            Assert.Equal("Index", result);
        }

        [Fact]
        public void Cannot_Delete_Non_Existing_User()
        {
            var accountController = new AccountController(_userManagerMock.Object, _userValidatorMock.Object, _passwordValidatorMock.Object, _passwordHasherMock.Object);

            var result = CastHelper.GetActionName(accountController.Delete("").Result);

            Assert.Null(result);
            _userManagerMock.Verify(m => m.DeleteAsync(It.IsAny<AppUser>()), Times.Never);
        }
    }
}