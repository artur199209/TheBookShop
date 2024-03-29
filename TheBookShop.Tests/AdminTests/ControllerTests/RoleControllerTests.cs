﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Areas.Admin.Model;
using TheBookShop.Models.DataModels;
using TheBookShop.Tests.Helper;
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

            var result = CastHelper.GetViewModel<IEnumerable<IdentityRole>>(roleController.Index()).ToArray();

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
            var actionName = CastHelper.GetActionName(result);
            _roleManagerMock.Verify(m => m.CreateAsync(It.IsAny<IdentityRole>()));

            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", actionName);
        }

        [Fact]
        public void Cannot_Create_Role_If_Already_Exists()
        {
            _roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            var roleController = new RoleController(_userManagerMock.Object, _roleManagerMock.Object);
            var result = CastHelper.GetActionName(roleController.Create(It.IsAny<string>()).Result);

            Assert.Null(result);
            _roleManagerMock.Verify(m => m.CreateAsync(It.IsAny<IdentityRole>()), Times.Never());
        }
        
        [Fact]
        public void Cannot_Create_Role_When_Name_Is_Null_Or_Empty()
        {
            var roleController = new RoleController(_userManagerMock.Object, _roleManagerMock.Object);
            roleController.ModelState.AddModelError("error", "error");
           
            var result = CastHelper.GetActionName(roleController.Create(null).Result);
            _roleManagerMock.Verify(m => m.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
            Assert.Null(result);
        }

        [Fact]
        public void Can_Delete_Existing_Role()
        {
            _roleManagerMock.Setup(x => x.DeleteAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole());
            var roleController = new RoleController(_userManagerMock.Object, _roleManagerMock.Object);

            var result = CastHelper.GetActionName(roleController.Delete(It.IsAny<string>()).Result);

            _roleManagerMock.Verify(m => m.DeleteAsync(It.IsAny<IdentityRole>()));
            Assert.Equal("Index", result);
        }

        [Fact]
        public void Cannot_Delete_Non_Existing_Role()
        {
            _roleManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((IdentityRole)null);
            var roleController = new RoleController(_userManagerMock.Object, _roleManagerMock.Object);

            var result = CastHelper.GetActionName(roleController.Delete(It.IsAny<string>()).Result);

            _roleManagerMock.Verify(m => m.DeleteAsync(It.IsAny<IdentityRole>()), Times.Never);
            Assert.Null(result);
        }

        [Fact]
        public void Role_Does_Not_Contain_Members()
        {
            var user1 = new AppUser { UserName = "user1" };
            var user2 = new AppUser { UserName = "user2" };
            _roleManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((IdentityRole)null);
            _userManagerMock.Setup(x => x.Users).Returns(new[]
            {
               user1, user2
            }.AsQueryable());

            _userManagerMock.Setup(x => x.IsInRoleAsync(user1, It.IsAny<string>())).ReturnsAsync(true);
            _userManagerMock.Setup(x => x.IsInRoleAsync(user2, It.IsAny<string>())).ReturnsAsync(true);
            _roleManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole());

            var roleController = new RoleController(_userManagerMock.Object, _roleManagerMock.Object);

            var result = CastHelper.GetViewModel<RoleEditModel>(roleController.Edit(It.IsAny<string>()).Result);

            Assert.NotNull(result);
            Assert.Equal(2, result.Members.Count());
            Assert.Empty(result.NonMembers);
        }

        [Fact]
        public void All_Users_Assigned_To_Role()
        {
            var user1 = new AppUser { UserName = "user1" };
            var user2 = new AppUser { UserName = "user2" };
            _roleManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((IdentityRole)null);
            _userManagerMock.Setup(x => x.Users).Returns(new[]
            {
                user1, user2
            }.AsQueryable());

            _userManagerMock.Setup(x => x.IsInRoleAsync(user1, It.IsAny<string>())).ReturnsAsync(false);
            _userManagerMock.Setup(x => x.IsInRoleAsync(user2, It.IsAny<string>())).ReturnsAsync(false);
            _roleManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole());

            var roleController = new RoleController(_userManagerMock.Object, _roleManagerMock.Object);

            var result = CastHelper.GetViewModel<RoleEditModel>(roleController.Edit(It.IsAny<string>()).Result);

            Assert.NotNull(result);
            Assert.Equal(2, result.NonMembers.Count());
            Assert.Empty(result.Members);
        }

        [Fact]
        public void Can_Assign_User_To_Role()
        {
            var model = new RoleModificationModel
            {
                RoleId = "1",
                RoleName = "role1",
                IdsToAdd = new [] { "1" }
            };

            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new AppUser());
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var roleController = new RoleController(_userManagerMock.Object, _roleManagerMock.Object);

            var result = CastHelper.GetActionName(roleController.Edit(model).Result);

            _userManagerMock.Verify(m => m.FindByIdAsync(It.IsAny<string>()));
            _userManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()));
            
            Assert.Equal("Index", result);
        }

        [Fact]
        public void Can_Remove_User_From_Role()
        {
            var model = new RoleModificationModel
            {
                RoleId = "1",
                RoleName = "role1",
                IdsToDelete = new[] { "1" }
            };

            _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new AppUser());
            _userManagerMock.Setup(x => x.RemoveFromRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var roleController = new RoleController(_userManagerMock.Object, _roleManagerMock.Object);

            var result = CastHelper.GetActionName(roleController.Edit(model).Result);

            _userManagerMock.Verify(m => m.FindByIdAsync(It.IsAny<string>()));
            _userManagerMock.Verify(m => m.RemoveFromRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()));

            Assert.Equal("Index", result);
        }

        [Fact]
        public void Cannot_Update_Role_When_Model_Is_Invalid()
        {
            var roleController = new RoleController(_userManagerMock.Object, _roleManagerMock.Object);
            roleController.ModelState.AddModelError("error", "error");

            var result = CastHelper.GetActionName(roleController.Edit(new RoleModificationModel()).Result);

            Assert.Null(result);
        }
    }
}