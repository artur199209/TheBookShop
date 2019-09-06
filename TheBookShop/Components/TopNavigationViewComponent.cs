using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace TheBookShop.Components
{
    public class TopNavigationViewComponent : ViewComponent
    {
        private readonly string[] _names = {"Top 100", "Nowości", "Zapowiedzi", "Nauka", "Przedsprzedaż"};
        
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
            topNav.Add(new NavbarItem() { Id = 1, Action = "About", NameOption = "Programowanie", Controller = "Home", IsParent = false, ParentId = -1 });
            topNav.Add(new NavbarItem() { Id = 2, Action = "Contact", NameOption = "Gry", Controller = "Home", IsParent = false, ParentId = -1 });
            // drop down Menu 
            topNav.Add(new NavbarItem() { Id = 3, Action = "Reports", NameOption = "Programowanie", Controller = "ReportGen", IsParent = true, ParentId = -1 });
            topNav.Add(new NavbarItem() { Id = 4, Action = "SummaryReport", NameOption = "Webowe", Controller = "ReportGen", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 5, Action = "DailyReport", NameOption = "Today Report", Controller = "ReportGen", IsParent = false, ParentId = 3 });
            topNav.Add(new NavbarItem() { Id = 6, Action = "MonthlyReport", NameOption = "Month Report", Controller = "ReportGen", IsParent = false, ParentId = 3 });
            // End drop down Menu
            topNav.Add(new NavbarItem() { Id = 7, Action = "Action", NameOption = "Bazy danych", Controller = "Home", IsParent = false, ParentId = -1 });
            return topNav;
        }
    }
}