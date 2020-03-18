using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [Authorize(Roles = "Administratorzy")]
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
            Log.Information("Getting all payment methods...");
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
                return RedirectToAction(nameof(Index));
            }

            return View(paymentMethod);
        }

        [Route("[action]")]
        public IActionResult Edit(int paymentMethodId)
        {
            var paymentMethod =
                _paymentMethodRepository.PaymentMethods.FirstOrDefault(x => x.PaymentMethodId == paymentMethodId);

            return View(paymentMethod);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public IActionResult Edit(PaymentMethod paymentMethod)
        {
            if (ModelState.IsValid)
            {
                _paymentMethodRepository.SavePaymentMethod(paymentMethod);

                return RedirectToAction(nameof(Index));
            }

            return Edit(paymentMethod.PaymentMethodId);
        }
    }
}