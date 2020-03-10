using Microsoft.AspNetCore.Mvc;

namespace TheBookShop.Controllers
{
    public class HelpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}