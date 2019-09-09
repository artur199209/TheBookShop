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
            topNav.Add(new NavbarItem() { Id = 1, Action = "Index", NameOption = "Programowanie", Controller = "Help", IsParent = false, ParentId = -1 });
            topNav.Add(new NavbarItem() { Id = 2, Action = "Contact", NameOption = "Gry", Controller = "Home", IsParent = false, ParentId = -1 });
            // drop down Menu 
            topNav.Add(new NavbarItem() { Id = 3, Action = "Index", NameOption = "Programowanie", Controller = "Help", IsParent = true, ParentId = -1 });
            topNav.Add(new NavbarItem() { Id = 4, Action = "Index", NameOption = ".NET/C#", Controller = "Cart", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 5, Action = "Index", NameOption = "Agile", Controller = "Help", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 6, Action = "Index", NameOption = "Algorytmy", Controller = "Help", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 7, Action = "Index", NameOption = "C/C++", Controller = "Help", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 8, Action = "Index", NameOption = "Delphi", Controller = "Help", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 9, Action = "Index", NameOption = "Go", Controller = "Help", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 10, Action = "Index", NameOption = "J2EE", Controller = "Help", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 11, Action = "Index", NameOption = "Java", Controller = "Help", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 12, Action = "Index", NameOption = "Objective-C/Swift", Controller = "Help", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 13, Action = "Index", NameOption = "Paradygmaty", Controller = "Help", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 14, Action = "Index", NameOption = "Perl", Controller = "Help", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 15, Action = "Index", NameOption = "Python", Controller = "Help", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 16, Action = "Index", NameOption = "Scala", Controller = "Help", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 17, Action = "Index", NameOption = "Visual Basic", Controller = "Help", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 18, Action = "Index", NameOption = "Visual Studio", Controller = "Help", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 19, Action = "Index", NameOption = "XAML", Controller = "Help", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 20, Action = "Index", NameOption = "Inne", Controller = "Help", IsParent = false, ParentId = 3 });
            // End drop down Menu
            topNav.Add(new NavbarItem() { Id = 21, Action = "Action", NameOption = "Bazy danych", Controller = "Home", IsParent = false, ParentId = -1 });

            topNav.Add(new NavbarItem() { Id = 22, Action = "Action", NameOption = "Nowości", Controller = "Home", IsParent = false, ParentId = -1 });
            topNav.Add(new NavbarItem() { Id = 23, Action = "Action", NameOption = "Zapowiedzi", Controller = "Home", IsParent = false, ParentId = -1 });
            topNav.Add(new NavbarItem() { Id = 24, Action = "Action", NameOption = "Wyprzedaż", Controller = "Home", IsParent = false, ParentId = -1 });

            return topNav;
        }
    }
}