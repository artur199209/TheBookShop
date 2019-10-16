using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Models;
using Xunit;

namespace TheBookShop.Tests.AdminTests.ControllerTests
{
    public class AccountControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _mockUser;
        private readonly Mock<IUserValidator<AppUser>> _mockUserValidator;
        private readonly Mock<IPasswordValidator<AppUser>> _mockPasswordValidator;
        private readonly Mock<IPasswordHasher<AppUser>> _mockPasswordHasher;
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
            _mockUser = new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _mockUserValidator = new Mock<IUserValidator<AppUser>>();
            _mockPasswordValidator = new Mock<IPasswordValidator<AppUser>>();
            _mockPasswordHasher = new Mock<IPasswordHasher<AppUser>>();
        }


        [Fact]
        public void Index_Contains_All_Users()
        {
            _mockUser.Setup(x => x.Users).Returns(new[]
            {
                new AppUser(),
                new AppUser(),
                new AppUser(),
                new AppUser()
            }.AsQueryable());

            var accountController = new AccountController(_mockUser.Object, _mockUserValidator.Object, _mockPasswordValidator.Object, _mockPasswordHasher.Object);
            
            var result = GetViewModel<IEnumerable<AppUser>>(accountController.Index());

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void Can_Edit_User()
        {
            _mockUser.Setup(userManager => userManager.FindByIdAsync("1"))
                .ReturnsAsync(new AppUser { Id = "1"});
            _mockUser.Setup(userManager => userManager.FindByIdAsync("2"))
                .ReturnsAsync(new AppUser { Id = "2"});
            _mockUser.Setup(userManager => userManager.FindByIdAsync("3"))
                .ReturnsAsync(new AppUser { Id = "3"});

            var accountController = new AccountController(_mockUser.Object, _mockUserValidator.Object, _mockPasswordValidator.Object, _mockPasswordHasher.Object);

            var user1 = GetViewModel<AppUser>(accountController.Edit("1").Result);
            var user2 = GetViewModel<AppUser>(accountController.Edit("2").Result);
            var user3 = GetViewModel<AppUser>(accountController.Edit("3").Result);

            Assert.Equal("1", user1?.Id);
            Assert.Equal("2", user2?.Id);
            Assert.Equal("3", user3?.Id);
        }

        [Fact]
        public void Cannot_Edit_Non_Existing_User()
        {
            _mockUser.Setup(userManager => userManager.FindByIdAsync("1"))
                .ReturnsAsync(new AppUser());
            _mockUser.Setup(userManager => userManager.FindByIdAsync("2"))
                .ReturnsAsync(new AppUser());

            var accountController = new AccountController(_mockUser.Object, _mockUserValidator.Object, _mockPasswordValidator.Object, _mockPasswordHasher.Object);

            var result = GetViewModel<AppUser>(accountController.Edit("3").Result);

            Assert.Null(result);
        }

        [Fact]
        public void Can_Create_New_User()
        {
            _mockUser.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            var accountController = new AccountController(_mockUser.Object, _mockUserValidator.Object, _mockPasswordValidator.Object, _mockPasswordHasher.Object);

            var createModel = new CreateModel
            {
                Name = "abc",
                Email = "www@wp.pl",
                Password = "Admin.123"
            };

            var result = accountController.Create(createModel).Result;
            _mockUser.Verify(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()));
            Assert.Equal("Index", GetActionName(result));
        }

        [Fact]
        public void Cannot_Create_User_When_Model_Is_Invalid()
        {
            var accountController = new AccountController(_mockUser.Object, _mockUserValidator.Object, _mockPasswordValidator.Object, _mockPasswordHasher.Object);

            accountController.ModelState.AddModelError("error", "error");
            var result = accountController.Create(null).Result;
            _mockUser.Verify(x => x.CreateAsync(It.IsAny<AppUser>()), Times.Never());
            Assert.IsType<ViewResult>(result);
            Assert.True(GetModelErrorsCount(result) > 0);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            _mockUser.Setup(userManager => userManager.FindByIdAsync("1234"))
                .ReturnsAsync(new AppUser { UserName = "abc", Email = "wwww.ss@ww.pl"});
            _mockUser.Setup(x => x.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);

            _mockUserValidator
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Success);
            _mockPasswordValidator
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            
            var accountController = new AccountController(_mockUser.Object,
              _mockUserValidator.Object,
               _mockPasswordValidator.Object, _mockPasswordHasher.Object);
            
            var result = (accountController.Edit(_appUser.Id, _appUser.Email, Password).Result);
            _mockUser.Verify(m => m.UpdateAsync(It.IsAny<AppUser>()));
            Assert.Equal("Index", GetActionName(result));
        }

        [Fact]
        public void Cannot_Save_Invalid_Data()
        {
            _mockUser.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new AppUser());
            _mockUser.Setup(x => x.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Failed());
            
            _mockUserValidator
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Failed());
            _mockPasswordValidator
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var accountController = new AccountController(_mockUser.Object,
                _mockUserValidator.Object,
                _mockPasswordValidator.Object, _mockPasswordHasher.Object);

            accountController.ModelState.AddModelError("error", "error");
            
            var result = accountController.Edit(_appUser.Id, _appUser.Email, Password).Result;
            
            Assert.True(GetModelErrorsCount(result) > 0);
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public void Cannot_Save_Non_Existing_User()
        {
            _mockUser.Setup(x => x.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            _mockUserValidator
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Success);
            _mockPasswordValidator
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<AppUser>>(), It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var accountController = new AccountController(_mockUser.Object,
                _mockUserValidator.Object,
                _mockPasswordValidator.Object, _mockPasswordHasher.Object);
            
            var result = (accountController.Edit(null, null, null).Result);
            Assert.IsType<ViewResult>(result);
            Assert.True(GetModelErrorsCount(result) > 0);
        }

        [Fact]
        public void Can_Delete_Existing_User()
        {
            _mockUser.Setup(x => x.DeleteAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            var accountController = new AccountController(_mockUser.Object, _mockUserValidator.Object, _mockPasswordValidator.Object, _mockPasswordHasher.Object);

            var result = accountController.Delete("1234").Result;

            Assert.Equal("Index", GetViewName(result));
        }

        [Fact]
        public void Cannot_Delete_Non_Existing_User()
        {
            var accountController = new AccountController(_mockUser.Object, _mockUserValidator.Object, _mockPasswordValidator.Object, _mockPasswordHasher.Object);

            var result = accountController.Delete("").Result;

            Assert.Equal("Index", GetViewName(result));
            Assert.True(GetModelErrorsCount(result) > 0);
        }


        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

        private string GetActionName(IActionResult result)
        {
            return (result as RedirectToActionResult)?.ActionName;
        }

        private string GetViewName(IActionResult result)
        {
            return (result as ViewResult)?.ViewName;
        }

        private int? GetModelErrorsCount(IActionResult result)
        {
            return (result as ViewResult)?.ViewData.ModelState.Count;
        }
    }
}