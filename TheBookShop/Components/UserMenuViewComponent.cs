using Microsoft.AspNetCore.Mvc;

namespace TheBookShop.Components
{
    public class UserMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}