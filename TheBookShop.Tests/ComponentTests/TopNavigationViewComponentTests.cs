using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
            var results = GetViewModel<IEnumerable<NavbarItem>>(target.Invoke()).ToArray();

            Assert.NotNull(target);
            Assert.NotNull(results);
        }

        [Fact]
        public void Top_Navigation_Panel_Contains_Parents()
        {
            var target = new TopNavigationViewComponent();
            var results = GetViewModel<IEnumerable<NavbarItem>>(target.Invoke()).ToArray();
            var parents = results.Where(x => x.IsParent);

            Assert.NotNull(parents);
        }

        [Fact]
        public void Top_Navigation_Panel_Contains_Childrens()
        {
            var target = new TopNavigationViewComponent();
            var results = GetViewModel<IEnumerable<NavbarItem>>(target.Invoke()).ToArray();
            var childrens = results.Where(x => x.IsParent);

            Assert.NotNull(childrens);
        }

        private T GetViewModel<T>(IViewComponentResult result) where T : class
        {
            return (result as ViewViewComponentResult)?.ViewData.Model as T;
        }
    }
}