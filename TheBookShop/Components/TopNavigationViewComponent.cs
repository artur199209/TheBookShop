using Microsoft.AspNetCore.Mvc;

namespace TheBookShop.Components
{
    public class TopNavigationViewComponent : ViewComponent
    {
        private readonly string[] _names = {"Top 100", "Nowości", "Zapowiedzi", "Nauka", "Przedsprzedaż"};
        
        public IViewComponentResult Invoke()
        {
            return View(_names);
        }
    }
}