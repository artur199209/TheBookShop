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
    public class RoleControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;

        public RoleControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            _userManagerMock = new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(mockRoleStore.Object, null, null, null, null);
        }

        [Fact]
        public void Index_Contains_All_Roles()
        {
            _roleManagerMock.Setup(x => x.Roles).Returns(new[]
            {
                new IdentityRole("Admin"),
                new IdentityRole("SuperAdmin"),
                new IdentityRole("User")
            }.AsQueryable());

            var roleController = new RoleController(_userManagerMock.Object, _roleManagerMock.Object);

            var result = GetViewModel<IEnumerable<IdentityRole>>(roleController.Index()).ToArray();

            Assert.NotNull(result);
            Assert.Equal(3, result.Length);
        }

        [Fact]
        public void Can_Add_New_RoleAsync()
        {
            _roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

            var roleController = new RoleController(_userManagerMock.Object, _roleManagerMock.Object);
            var result = roleController.Create(It.IsAny<string>()).Result;

            _roleManagerMock.Verify(m => m.CreateAsync(It.IsAny<IdentityRole>()));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", GetActionName(result));
        }

        [Fact]
        public void Cannot_Create_Role_If_Already_Exists()
        {
            _roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            var roleController = new RoleController(_userManagerMock.Object, _roleManagerMock.Object);
            var result = GetActionName(roleController.Create(It.IsAny<string>()).Result);

            Assert.Null(result);
            _roleManagerMock.Verify(m => m.CreateAsync(It.IsAny<IdentityRole>()), Times.Never());
        }
        
        [Fact]
        public void Cannot_Create_Role_When_Name_Is_Null_Or_Empty()
        {
            var roleController = new RoleController(_userManagerMock.Object, _roleManagerMock.Object);
            roleController.ModelState.AddModelError("error", "error");
           
            var result = GetActionName(roleController.Create(null).Result);
            _roleManagerMock.Verify(m => m.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
            Assert.Null(result);
        }

        [Fact]
        public void Can_Delete_Existing_Role()
        {
            _roleManagerMock.Setup(x => x.DeleteAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole());
            var roleController = new RoleController(_userManagerMock.Object, _roleManagerMock.Object);

            var result = GetActionName(roleController.Delete(It.IsAny<string>()).Result);

            _roleManagerMock.Verify(m => m.DeleteAsync(It.IsAny<IdentityRole>()));
            Assert.Equal("Index", result);
        }

        [Fact]
        public void Cannot_Delete_Non_Existing_Role()
        {
            _roleManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((IdentityRole)null);
            var roleController = new RoleController(_userManagerMock.Object, _roleManagerMock.Object);

            var result = GetActionName(roleController.Delete(It.IsAny<string>()).Result);

            _roleManagerMock.Verify(m => m.DeleteAsync(It.IsAny<IdentityRole>()), Times.Never);
            Assert.Null(result);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

        private string GetActionName(IActionResult result)
        {
            return (result as RedirectToActionResult)?.ActionName;
        }
    }
}