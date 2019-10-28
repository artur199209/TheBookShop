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
        public bool IsActive { get; set; }
    }


    public class LeftNav
    {
        public IEnumerable<LeftNavItem> LeftPanel()
        {
            var topNav = new List<LeftNavItem>();
            topNav.Add(new LeftNavItem() { Id = 1, NameOption = "Home", IsParent = true, ParentId = -1, IconClass = "fas fa-home", IsActive = true});
            topNav.Add(new LeftNavItem() { Id = 2, Area = "Admin", Controller = "Admin", Action = "Index", NameOption = "Home", IsParent = false, ParentId = 1, IconClass = "fa fa-circle-o" });

            topNav.Add(new LeftNavItem() { Id = 3, NameOption = "Role", IsParent = true, ParentId = -1, IconClass = "fas fa-layer-group" });
            topNav.Add(new LeftNavItem() { Id = 4, Area = "Admin", Controller = "Role", Action = "Index", NameOption = "Wszystkie", IsParent = false, ParentId = 3, IconClass = "fa fa-circle-o" });
            topNav.Add(new LeftNavItem() { Id = 5, Area = "Admin", Controller = "Role", Action = "Create", NameOption = "Dodaj", IsParent = false, ParentId = 3, IconClass = "fa fa-circle-o" });

            topNav.Add(new LeftNavItem() { Id = 6, NameOption = "Użytkownicy", IsParent = true, ParentId = -1, IconClass = "fas fa-users" });
            topNav.Add(new LeftNavItem() { Id = 7, Area = "Admin", Controller = "Account", Action = "Index", NameOption = "Wszyscy", IsParent = false, ParentId = 6, IconClass = "fa fa-circle-o" });
            topNav.Add(new LeftNavItem() { Id = 8, Area = "Admin", Controller = "Account", Action = "Create", NameOption = "Dodaj", IsParent = false, ParentId = 6, IconClass = "fa fa-circle-o" });

            topNav.Add(new LeftNavItem() { Id = 9, NameOption = "Autorzy", IsParent = true, ParentId = -1, IconClass = "fas fa-users" });
            topNav.Add(new LeftNavItem() { Id = 10, Area = "Admin", Controller = "Author", Action = "Index", NameOption = "Wszyscy", IsParent = false, ParentId = 9, IconClass = "fa fa-circle-o" });
            topNav.Add(new LeftNavItem() { Id = 11, Area = "Admin", Controller = "Author", Action = "Create", NameOption = "Dodaj", IsParent = false, ParentId = 9, IconClass = "fa fa-circle-o" });

            topNav.Add(new LeftNavItem() { Id = 12, NameOption = "Pozycje", IsParent = true, ParentId = -1, IconClass = "fas fa-book" });
            topNav.Add(new LeftNavItem() { Id = 13, Area = "Admin", Controller = "Product", Action = "Index", NameOption = "Wszystkie", IsParent = false, ParentId = 12, IconClass = "fa fa-circle-o" });
            topNav.Add(new LeftNavItem() { Id = 14, Area = "Admin", Controller = "Product", Action = "Create", NameOption = "Dodaj", IsParent = false, ParentId = 12, IconClass = "fa fa-circle-o" });

            topNav.Add(new LeftNavItem() { Id = 15, NameOption = "Zamówienia", IsParent = true, ParentId = -1, IconClass = "fab fa-first-order" });
            topNav.Add(new LeftNavItem() { Id = 16, Area = "Admin", Action = "Index", NameOption = "Wszystkie", Controller = "Order", IsParent = false, ParentId = 15, IconClass = "fa fa-circle-o" });
            topNav.Add(new LeftNavItem() { Id = 17, Area = "Admin", Action = "Completed", NameOption = "Zakończone", Controller = "Order", IsParent = false, ParentId = 15, IconClass = "fa fa-circle-o" });
            topNav.Add(new LeftNavItem() { Id = 18, Area = "Admin", Action = "NotCompleted", NameOption = "W trakcie", Controller = "Order", IsParent = false, ParentId = 15, IconClass = "fa fa-circle-o" });

            topNav.Add(new LeftNavItem() { Id = 19, NameOption = "Płatności", IsParent = true, ParentId = -1, IconClass = "fas fa-receipt" });
            topNav.Add(new LeftNavItem() { Id = 20, Area = "Admin", Controller = "Payment", Action = "Index", NameOption = "Wszystkie", IsParent = false, ParentId = 19, IconClass = "fa fa-circle-o" });
            return topNav;
        }
    }
}