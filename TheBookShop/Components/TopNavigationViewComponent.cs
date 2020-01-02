using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace TheBookShop.Components
{
    public class TopNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            Navbar navbar = new Navbar();

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
        public IEnumerable<NavbarItem> NavbarTop()
        {
            var topNav = new List<NavbarItem>();
            //topNav.Add(new NavbarItem() { Id = 1, Action = "Index", NameOption = "Programowanie", Controller = "Help", IsParent = false, ParentId = -1 });
            //topNav.Add(new NavbarItem() { Id = 2, Action = "Contact", NameOption = "Gry", Controller = "Home", IsParent = false, ParentId = -1 });
            // drop down Menu 
            topNav.Add(new NavbarItem() { Id = 3, Action = "Index", NameOption = "Programowanie", Controller = "Product", IsParent = true, ParentId = -1 });
            topNav.Add(new NavbarItem() { Id = 4, Action = "List", NameOption = ".NET/C#", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 5, Action = "List", NameOption = "Agile", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 6, Action = "List", NameOption = "Algorytmy", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 7, Action = "List", NameOption = "C/C++", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 8, Action = "List", NameOption = "Delphi", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 9, Action = "List", NameOption = "Go", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 10, Action = "List", NameOption = "J2EE", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 11, Action = "List", NameOption = "Java", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 12, Action = "List", NameOption = "Objective-C/Swift", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 13, Action = "List", NameOption = "Paradygmaty", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 14, Action = "List", NameOption = "Perl", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 15, Action = "List", NameOption = "Python", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 16, Action = "List", NameOption = "Scala", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 17, Action = "List", NameOption = "Visual Basic", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 18, Action = "List", NameOption = "Visual Studio", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 19, Action = "List", NameOption = "XAML", Controller = "Product", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 20, Action = "List", NameOption = "Inne", Controller = "Product", IsParent = false, ParentId = 3 });
            // End drop down Menu
            topNav.Add(new NavbarItem() { Id = 21, Action = "List", NameOption = "Bazy danych", Controller = "Product", IsParent = false, ParentId = -1 });

            topNav.Add(new NavbarItem() { Id = 22, Action = "Action", NameOption = "Nowości", Controller = "Home", IsParent = false, ParentId = -1 });
            topNav.Add(new NavbarItem() { Id = 23, Action = "Action", NameOption = "Zapowiedzi", Controller = "Home", IsParent = false, ParentId = -1 });
            topNav.Add(new NavbarItem() { Id = 24, Action = "Action", NameOption = "Wyprzedaż", Controller = "Home", IsParent = false, ParentId = -1 });

            return topNav;
        }
    }
}