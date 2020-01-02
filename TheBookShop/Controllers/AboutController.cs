using Microsoft.AspNetCore.Mvc;

namespace TheBookShop.Controllers
{
    [Route("[controller]")]
    public class AboutController : Controller
    {
        [Route("")]
        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("[action]")]
        public ViewResult DeliveryMethods()
        {
            return View();
        }

        [Route("[action]")]
        public ViewResult PaymentMethods()
        {
            return View();
        }
    }
}