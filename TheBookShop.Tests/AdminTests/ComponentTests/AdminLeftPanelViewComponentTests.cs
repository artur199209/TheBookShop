using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using TheBookShop.Areas.Admin.Components;
using Xunit;

namespace TheBookShop.Tests.AdminTests.ComponentTests
{
    public class AdminLeftPanelViewComponentTests
    {
        [Fact]
        public void Admin_Left_Panel_Is_Not_Null()
        {
            var target = new AdminLeftPanelViewComponent();
            var results = ((IEnumerable<LeftNavItem>)(target.Invoke() as ViewViewComponentResult)?.ViewData.Model)?.ToArray();
            Assert.NotNull(target);
            Assert.NotNull(results);
        }

        [Fact]
        public void Admin_Left_Panel_Contains_Parent_Items()
        {
            var target = new AdminLeftPanelViewComponent();
            var results = ((IEnumerable<LeftNavItem>)(target.Invoke() as ViewViewComponentResult)?.ViewData.Model)?.ToArray();
            
            Assert.True(results?.Where(x => x.IsParent) != null);
        }

        [Fact]
        public void Admin_Left_Panel_Contains_Childrens_Items()
        {
            var target = new AdminLeftPanelViewComponent();
            var results = ((IEnumerable<LeftNavItem>)(target.Invoke() as ViewViewComponentResult)?.ViewData.Model)?.ToArray();

            Assert.True(results?.Where(x => !x.IsParent) != null);
        }

    }
}