using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace TheBookShop.Areas.Admin.Components
{
    public class AdminLeftPanelViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var leftNav = new LeftNav();
            return View(leftNav.LeftPanel());
        }
    }

    public class LeftNavItem
    {
        public int Id { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
        public int ParentId { get; set; }
        public bool IsParent { get; set; }
        public string NameOption { get; set; }
        public string IconClass { get; set; }
    }


    public class LeftNav
    {
        public IEnumerable<LeftNavItem> LeftPanel()
        {
            var topNav = new List<LeftNavItem>();
            topNav.Add(new LeftNavItem() { Id = 1, Area = "Admin", Action = "Index", NameOption = "Home Management", Controller = "Admin", IsParent = true, ParentId = -1, IconClass = "fa fa-dashboard" });
            topNav.Add(new LeftNavItem() { Id = 2, Area = "Admin", Action = "Index", NameOption = "Home Index", Controller = "Admin", IsParent = false, ParentId = 1, IconClass = "fa fa-circle-o" });

            topNav.Add(new LeftNavItem() { Id = 3, Area = "Admin", Action = "Index", NameOption = "Użytkownicy", Controller = "Admin", IsParent = true, ParentId = -1, IconClass = "fas fa-users" });
            topNav.Add(new LeftNavItem() { Id = 4, Area = "Admin", Action = "Index", NameOption = "Wszyscy", Controller = "Admin", IsParent = false, ParentId = 3, IconClass = "fa fa-circle-o" });
            topNav.Add(new LeftNavItem() { Id = 5, Area = "Admin", Action = "Index", NameOption = "Dodaj", Controller = "Admin", IsParent = false, ParentId = 3, IconClass = "fa fa-circle-o" });

            topNav.Add(new LeftNavItem() { Id = 6, Area = "Admin", Action = "Index", NameOption = "Autorzy", Controller = "Author", IsParent = true, ParentId = -1, IconClass = "fas fa-users" });
            topNav.Add(new LeftNavItem() { Id = 7, Area = "Admin", Action = "Index", NameOption = "Wszyscy", Controller = "Author", IsParent = false, ParentId = 6, IconClass = "fa fa-circle-o" });
            topNav.Add(new LeftNavItem() { Id = 8, Area = "Admin", Action = "Create", NameOption = "Dodaj", Controller = "Author", IsParent = false, ParentId = 6, IconClass = "fa fa-circle-o" });

            topNav.Add(new LeftNavItem() { Id = 9, Area = "Admin", Action = "Index", NameOption = "Pozycje", Controller = "Product", IsParent = true, ParentId = -1, IconClass = "fas fa-book" });
            topNav.Add(new LeftNavItem() { Id = 10, Area = "Admin", Action = "Index", NameOption = "Wszystkie", Controller = "Product", IsParent = false, ParentId = 9, IconClass = "fa fa-circle-o" });
            topNav.Add(new LeftNavItem() { Id = 11, Area = "Admin", Action = "Create", NameOption = "Dodaj", Controller = "Product", IsParent = false, ParentId = 9, IconClass = "fa fa-circle-o" });

            topNav.Add(new LeftNavItem() { Id = 12, Area = "Admin", Action = "Index", NameOption = "Zamówienia", Controller = "Order", IsParent = true, ParentId = -1, IconClass = "fab fa-first-order" });
            topNav.Add(new LeftNavItem() { Id = 13, Area = "Admin", Action = "Index", NameOption = "Wszystkie", Controller = "Order", IsParent = false, ParentId = 12, IconClass = "fa fa-circle-o" });
            topNav.Add(new LeftNavItem() { Id = 14, Area = "Admin", Action = "Index", NameOption = "Zakończone", Controller = "Order", IsParent = false, ParentId = 12, IconClass = "fa fa-circle-o" });
            topNav.Add(new LeftNavItem() { Id = 15, Area = "Admin", Action = "Index", NameOption = "W trakcie", Controller = "Order", IsParent = false, ParentId = 12, IconClass = "fa fa-circle-o" });

            return topNav;
        }
    }
}