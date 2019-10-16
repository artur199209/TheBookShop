using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using TheBookShop.Areas.Admin.Infrastructure;
using TheBookShop.Models;
using Xunit;

namespace TheBookShop.Tests.AdminTests.InfrastructureTests
{
    public class RoleUsersTagHelperTests
    {
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;

        private readonly TagHelperOutput _output;
        private readonly TagHelperContext _ctx;

        public RoleUsersTagHelperTests()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            var mockRoleStore = new Mock<IRoleStore<IdentityRole>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(mockRoleStore.Object, null, null, null, null);

            var mockContent = new Mock<TagHelperContent>();

            _ctx = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(), "");

            _output = new TagHelperOutput("td",
                new TagHelperAttributeList(),
                (cache, encoder) => Task.FromResult(mockContent.Object));
        }

        [Fact]
        public async Task Empty_List_Of_Users_When_Role_Does_Not_Contain_Any_Users()
        {
            var rolesUsersTagHelper = new RoleUsersTagHelper(_mockUserManager.Object, _mockRoleManager.Object);
            
            await rolesUsersTagHelper.ProcessAsync(_ctx, _output);
            
            Assert.NotNull(_output.Content);
            Assert.Equal("Brak kont", _output.Content.GetContent());
        }

        [Fact]
        public async Task Output_Contains_List_Of_Users()
        {
            var appUser1 = new AppUser { UserName = "admin1" };
            var appUser2 = new AppUser { UserName = "admin2" };

            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new IdentityRole("abc"));
            _mockUserManager.Setup(x => x.IsInRoleAsync(appUser1, "abc")).ReturnsAsync(true);
            _mockUserManager.Setup(x => x.IsInRoleAsync(appUser2, "abc")).ReturnsAsync(true);

            _mockUserManager.Setup(x => x.Users).Returns(new[]
            {
                appUser1, appUser2
            }.AsQueryable());

            var rolesUsersTagHelper = new RoleUsersTagHelper(_mockUserManager.Object, _mockRoleManager.Object);
            
            await rolesUsersTagHelper.ProcessAsync(_ctx, _output);

            Assert.NotNull(_output);
            Assert.Equal(string.Join(", ", _mockUserManager.Object.Users), _output.Content.GetContent());
        }

    }
}