using Microsoft.AspNetCore.Mvc;

namespace TheBookShop.Controllers
{
    public class NewsletterController : Controller
    {
        public IActionResult Subscribe()
        {
            return View();
        }
    }
}