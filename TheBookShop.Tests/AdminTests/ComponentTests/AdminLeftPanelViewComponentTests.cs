using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
            var results = GetViewModel<IEnumerable<LeftNavItem>>(target.Invoke())?.ToArray();

            Assert.NotNull(target);
            Assert.NotNull(results);
        }

        [Fact]
        public void Admin_Left_Panel_Contains_Parents()
        {
            var target = new AdminLeftPanelViewComponent();
            var results = GetViewModel<IEnumerable<LeftNavItem>>(target.Invoke())?.ToArray();

            var parents = results?.Where(x => x.IsParent);
            Assert.NotNull(parents);
        }

        [Fact]
        public void Admin_Left_Panel_Contains_Childrens()
        {
            var target = new AdminLeftPanelViewComponent();
            var results = GetViewModel<IEnumerable<LeftNavItem>>(target.Invoke())?.ToArray();

            var childrens = results?.Where(x => !x.IsParent);
            Assert.NotNull(childrens);
        }

        private T GetViewModel<T>(IViewComponentResult result) where T : class
        {
            return (result as ViewViewComponentResult)?.ViewData.Model as T;
        }

    }
}