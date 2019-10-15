using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            return View(_paymentRepository.Payments);
        }
    }
}