using System.Collections.Generic;
using System.Linq;
using TheBookShop.Areas.Admin.Components;
using TheBookShop.Tests.Helper;
using Xunit;

namespace TheBookShop.Tests.AdminTests.ComponentTests
{
    public class AdminLeftPanelViewComponentTests
    {
        [Fact]
        public void Admin_Left_Panel_Is_Not_Null()
        {
            var target = new AdminLeftPanelViewComponent();
            var results = CastHelper.GetViewComponentModel<IEnumerable<LeftNavItem>>(target.Invoke())?.ToArray();

            Assert.NotNull(target);
            Assert.NotNull(results);
        }

        [Fact]
        public void Admin_Left_Panel_Contains_Parents()
        {
            var target = new AdminLeftPanelViewComponent();
            var results = CastHelper.GetViewComponentModel<IEnumerable<LeftNavItem>>(target.Invoke())?.ToArray();

            var parents = results?.Where(x => x.IsParent);
            Assert.NotNull(parents);
        }

        [Fact]
        public void Admin_Left_Panel_Contains_Childrens()
        {
            var target = new AdminLeftPanelViewComponent();
            var results = CastHelper.GetViewComponentModel<IEnumerable<LeftNavItem>>(target.Invoke())?.ToArray();

            var childrens = results?.Where(x => !x.IsParent);
            Assert.NotNull(childrens);
        }
    }
}