using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class PaymentMethodController : Controller
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodController(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            return View(_paymentMethodRepository.PaymentMethods);
        }

        [Route("[action]")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public IActionResult Create(PaymentMethod paymentMethod)
        {
            if (ModelState.IsValid)
            {
                _paymentMethodRepository.SavePaymentMethod(paymentMethod);
                RedirectToAction(nameof(Index));
            }

            return View(paymentMethod);
        }
    }
}