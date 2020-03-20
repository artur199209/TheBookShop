using Microsoft.AspNetCore.Mvc;

namespace TheBookShop.Controllers
{
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        [Route("[action]")]
        public IActionResult Error()
        {
            return View();
        }
    }
}