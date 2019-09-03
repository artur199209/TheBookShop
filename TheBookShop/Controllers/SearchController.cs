using Microsoft.AspNetCore.Mvc;

namespace TheBookShop.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Search()
        {
            return View();
        }
    }
}