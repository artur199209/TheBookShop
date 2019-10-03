using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using TheBookShop.Components;
using Xunit;

namespace TheBookShop.Tests.ComponentTests
{
    public class TopNavigationViewComponentTests
    {
        [Fact]
        public void Top_Navigation_Panel_Is_Not_Null()
        {
            var target = new TopNavigationViewComponent();
            var results = ((IEnumerable<NavbarItem>)(target.Invoke() as ViewViewComponentResult)?.ViewData.Model)?.ToArray();
            Assert.NotNull(target);
            Assert.NotNull(results);
        }

        [Fact]
        public void Top_Navigation_Panel_Contains_Parent_Items()
        {
            var target = new TopNavigationViewComponent();
            var results = ((IEnumerable<NavbarItem>)(target.Invoke() as ViewViewComponentResult)?.ViewData.Model)?.ToArray();

            Assert.True(results?.Where(x => x.IsParent) != null);
        }

        [Fact]
        public void Top_Navigation_Panel_Contains_Childrens_Items()
        {
            var target = new TopNavigationViewComponent();
            var results = ((IEnumerable<NavbarItem>)(target.Invoke() as ViewViewComponentResult)?.ViewData.Model)?.ToArray();

            Assert.True(results?.Where(x => !x.IsParent) != null);
        }
    }
}