using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Models;
using Xunit;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace TheBookShop.Tests.AdminTests.ControllerTests
{
    public class AccountControllerTests
    {
        private readonly Mock<UserManager<IdentityUser>> _mockUser;

        public AccountControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _mockUser = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        }


        [Fact]
        public void Index_Contains_All_Users()
        {
            _mockUser.Setup(x => x.Users).Returns(new[]
            {
                new IdentityUser("abc1"),
                new IdentityUser("abc2"),
                new IdentityUser("abc3"),
                new IdentityUser("abc4")
            }.AsQueryable());

            var accountController = new AccountController(_mockUser.Object, new Mock<IUserValidator<IdentityUser>>().Object,
                new Mock<IPasswordValidator<IdentityUser>>().Object, new Mock<IPasswordHasher<IdentityUser>>().Object);
            
            var result = GetViewModel<IEnumerable<IdentityUser>>(accountController.Index());

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void Can_Edit_User()
        {
            _mockUser.Setup(userManager => userManager.FindByIdAsync("1"))
                .ReturnsAsync(new IdentityUser { Id = "1"});
            _mockUser.Setup(userManager => userManager.FindByIdAsync("2"))
                .ReturnsAsync(new IdentityUser { Id = "2"});
            _mockUser.Setup(userManager => userManager.FindByIdAsync("3"))
                .ReturnsAsync(new IdentityUser { Id = "3"});

            var accountController = new AccountController(_mockUser.Object, new Mock<IUserValidator<IdentityUser>>().Object,
                new Mock<IPasswordValidator<IdentityUser>>().Object, new Mock<IPasswordHasher<IdentityUser>>().Object);
            
            var user1 = GetViewModel<IdentityUser>(accountController.Edit("1").Result);
            var user2 = GetViewModel<IdentityUser>(accountController.Edit("2").Result);
            var user3 = GetViewModel<IdentityUser>(accountController.Edit("3").Result);

            Assert.Equal("1", user1?.Id);
            Assert.Equal("2", user2?.Id);
            Assert.Equal("3", user3?.Id);
        }

        [Fact]
        public void Cannot_Edit_Non_Existing_User()
        {
            _mockUser.Setup(userManager => userManager.FindByIdAsync("1"))
                .ReturnsAsync(new IdentityUser());
            _mockUser.Setup(userManager => userManager.FindByIdAsync("2"))
                .ReturnsAsync(new IdentityUser());

            var accountController = new AccountController(_mockUser.Object,
                new Mock<IUserValidator<IdentityUser>>().Object,
                new Mock<IPasswordValidator<IdentityUser>>().Object, new Mock<IPasswordHasher<IdentityUser>>().Object);
            
            var result = GetViewModel<IdentityUser>(accountController.Edit("3").Result);

            Assert.Null(result);
        }

        [Fact]
        public void Can_Create_New_User()
        {
            _mockUser.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            var accountController = new AccountController(_mockUser.Object,
                new Mock<IUserValidator<IdentityUser>>().Object,
                new Mock<IPasswordValidator<IdentityUser>>().Object, new Mock<IPasswordHasher<IdentityUser>>().Object);

            var createModel = new CreateModel
            {
                Name = "abc",
                Email = "www@wp.pl",
                Password = "Admin.123"
            };

            var result = accountController.Create(createModel).Result;
            _mockUser.Verify(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()));
            Assert.Equal("Index", GetActionName(result));
        }

        [Fact]
        public void Cannot_Create_User_When_Model_Is_Invalid()
        {
            var accountController = new AccountController(_mockUser.Object,
                new Mock<IUserValidator<IdentityUser>>().Object,
                new Mock<IPasswordValidator<IdentityUser>>().Object, new Mock<IPasswordHasher<IdentityUser>>().Object);
            
            accountController.ModelState.AddModelError("error", "error");
            var result = accountController.Create(null).Result;
            _mockUser.Verify(x => x.CreateAsync(It.IsAny<IdentityUser>()), Times.Never());
            Assert.IsType<ViewResult>(result);
            Assert.True(GetModelErrorsCount(result) > 0);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            _mockUser.Setup(userManager => userManager.FindByIdAsync("1234"))
                .ReturnsAsync(new IdentityUser { UserName = "abc", Email = "wwww.ss@ww.pl"});
            _mockUser.Setup(x => x.UpdateAsync(It.IsAny<IdentityUser>())).ReturnsAsync(IdentityResult.Success);
            var mockUserValidator = new Mock<IUserValidator<IdentityUser>>();
            mockUserValidator
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<IdentityUser>>(), It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);
            var mockPasswordValidator = new Mock<IPasswordValidator<IdentityUser>>();
            mockPasswordValidator
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<IdentityUser>>(), It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            
            var accountController = new AccountController(_mockUser.Object,
              mockUserValidator.Object,
               mockPasswordValidator.Object, new Mock<IPasswordHasher<IdentityUser>>().Object);

            IdentityUser identityUser = new IdentityUser
            { 
                UserName = "abcd",
                Id = "1234",
                Email = "wwww.ss@ww.pl"
            };

            string password = "admin123";

            var result = (accountController.Edit(identityUser.Id, identityUser.Email, password).Result);
            _mockUser.Verify(m => m.UpdateAsync(It.IsAny<IdentityUser>()));
            Assert.Equal("Index", GetActionName(result));
        }

        [Fact]
        public void Cannot_Save_Invalid_Data()
        {
            _mockUser.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityUser());
            _mockUser.Setup(x => x.UpdateAsync(It.IsAny<IdentityUser>())).ReturnsAsync(IdentityResult.Failed());

            var mockUserValidator = new Mock<IUserValidator<IdentityUser>>();
            mockUserValidator
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<IdentityUser>>(), It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Failed());
            var mockPasswordValidator = new Mock<IPasswordValidator<IdentityUser>>();
            mockPasswordValidator
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<IdentityUser>>(), It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var accountController = new AccountController(_mockUser.Object,
                mockUserValidator.Object,
                mockPasswordValidator.Object, new Mock<IPasswordHasher<IdentityUser>>().Object);

            accountController.ModelState.AddModelError("error", "error");

            IdentityUser identityUser = new IdentityUser
            {
                UserName = "abcd",
                Id = "1234",
                Email = "wwww.ss@ww.pl"
            };

            string password = "admin123";

            var result = accountController.Edit(identityUser.Id, identityUser.Email, password).Result;
            
            Assert.True(GetModelErrorsCount(result) > 0);
            Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public void Cannot_Save_Non_Existing_User()
        {
            _mockUser.Setup(x => x.UpdateAsync(It.IsAny<IdentityUser>())).ReturnsAsync(IdentityResult.Success);
            var mockUserValidator = new Mock<IUserValidator<IdentityUser>>();
            mockUserValidator
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<IdentityUser>>(), It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);
            var mockPasswordValidator = new Mock<IPasswordValidator<IdentityUser>>();
            mockPasswordValidator
                .Setup(x => x.ValidateAsync(It.IsAny<UserManager<IdentityUser>>(), It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var accountController = new AccountController(_mockUser.Object,
                mockUserValidator.Object,
                mockPasswordValidator.Object, new Mock<IPasswordHasher<IdentityUser>>().Object);
            
            var result = (accountController.Edit(null, null, null).Result);
            Assert.IsType<ViewResult>(result);
            Assert.True(GetModelErrorsCount(result) > 0);
        }

        [Fact]
        public void Can_Delete_Existing_User()
        {
            _mockUser.Setup(x => x.DeleteAsync(It.IsAny<IdentityUser>())).ReturnsAsync(IdentityResult.Success);
            var accountController = new AccountController(_mockUser.Object,
                new Mock<IUserValidator<IdentityUser>>().Object,
                new Mock<IPasswordValidator<IdentityUser>>().Object, new Mock<IPasswordHasher<IdentityUser>>().Object);

            var result = accountController.Delete("1234").Result;

            Assert.Equal("Index", GetViewName(result));
        }

        [Fact]
        public void Cannot_Delete_Non_Existing_User()
        {
            var accountController = new AccountController(_mockUser.Object,
                new Mock<IUserValidator<IdentityUser>>().Object,
                new Mock<IPasswordValidator<IdentityUser>>().Object, new Mock<IPasswordHasher<IdentityUser>>().Object);

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