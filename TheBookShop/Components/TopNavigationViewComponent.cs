using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;

namespace TheBookShop.Components
{
    public class TopNavigationViewComponent : ViewComponent
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public TopNavigationViewComponent(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public IViewComponentResult Invoke()
        {
            var navbar = new Navbar(_productCategoryRepository.ProductCategories);

            return View(navbar.NavbarTop());
        }
    }

    public class NavbarItem
    {
        public int Id { get; set; }
        public string NameOption { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool HavingImageClass { get; set; }
        public string CssClass { get; set; }
        public int ParentId { get; set; }
        public bool IsParent { get; set; }
    }
    public class Navbar
    {
        private readonly IEnumerable<ProductCategory> _productCategories;

        public Navbar(IEnumerable<ProductCategory> productCategories)
        {
            _productCategories = productCategories;
        }

        public IEnumerable<NavbarItem> NavbarTop()
        {
            var topNav = new List<NavbarItem>
            {
                new NavbarItem
                {
                    Id = 1,
                    Action = "Index",
                    NameOption = "Programowanie",
                    Controller = "Product",
                    IsParent = true,
                    ParentId = -1
                }
            };

            foreach (var category in _productCategories)
            {
                topNav.Add(new NavbarItem { Id = topNav.Max(x => x.Id) + 1, Action = "List", NameOption = category.Name, Controller = "Product", IsParent = false, ParentId = 1 });
            }
            
            topNav.Add(new NavbarItem { Id = topNav.Max(x => x.Id) + 1, Action = "Action", NameOption = "Nowości", Controller = "Home", IsParent = false, ParentId = -1 });
            topNav.Add(new NavbarItem { Id = topNav.Max(x => x.Id) + 1, Action = "Action", NameOption = "Zapowiedzi", Controller = "Home", IsParent = false, ParentId = -1 });
            topNav.Add(new NavbarItem { Id = topNav.Max(x => x.Id) + 1, Action = "Action", NameOption = "Wyprzedaż", Controller = "Home", IsParent = false, ParentId = -1 });

            return topNav;
        }
    }
}