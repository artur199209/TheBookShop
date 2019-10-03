using Microsoft.AspNetCore.Mvc;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("")]
        [Route("[action]")]
        public IActionResult Login()
        {
            return View();
        }
    }
}