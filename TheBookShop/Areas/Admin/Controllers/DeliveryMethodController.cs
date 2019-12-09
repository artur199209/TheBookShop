using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TheBookShop.Areas.Admin.Model;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class DeliveryMethodController : Controller
    {
        private readonly IDeliveryMethodRepository _deliveryMethodRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public DeliveryMethodController(IDeliveryMethodRepository deliveryMethodRepository, IPaymentMethodRepository paymentMethodRepository)
        {
            _deliveryMethodRepository = deliveryMethodRepository;
            _paymentMethodRepository = paymentMethodRepository;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            return View(_deliveryMethodRepository.DeliveryMethods);
        }

        [Route("[action]")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public IActionResult Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                var deliveryMethod = new DeliveryMethod
                {
                    Name = name
                };

                _deliveryMethodRepository.SaveDeliveryMethod(deliveryMethod);
                TempData["message"] = $"{deliveryMethod.Name} has been saved";
                return RedirectToAction(nameof(Index));
            }

            return View(nameof(Index));
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Edit(int deliveryMethodId)
        {
            var deliveryMethod = _deliveryMethodRepository.DeliveryMethods
                .FirstOrDefault(x => x.DeliveryMethodId == deliveryMethodId);

            var deliveryPaymentViewModel = new DeliveryPaymentViewModel
            {
                DeliveryMethod = deliveryMethod,
                PaymentMethods = _paymentMethodRepository.PaymentMethods.Except(deliveryMethod?.PaymentMethods).ToList()
            };

            return View(deliveryPaymentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public IActionResult Edit(DeliveryPaymentModificationModel deliveryPaymentModificationModel)
        {
            if (ModelState.IsValid)
            {
                deliveryPaymentModificationModel.DeliveryMethod.PaymentMethods = new List<PaymentMethod>();

                foreach (int id in deliveryPaymentModificationModel.IdsToAdd ?? new List<int>())
                {
                    var paymentMethod =
                        _paymentMethodRepository.PaymentMethods.FirstOrDefault(x => x.PaymentMethodId == id);
                    deliveryPaymentModificationModel.DeliveryMethod.PaymentMethods.Add(paymentMethod);
                }

                foreach (int id in deliveryPaymentModificationModel.IdsToDelete ?? new List<int>())
                {
                    var paymentMethod =
                        _paymentMethodRepository.PaymentMethods.FirstOrDefault(x => x.PaymentMethodId == id);
                    var result = deliveryPaymentModificationModel.DeliveryMethod.PaymentMethods.Remove(paymentMethod);

                    ModelState.AddModelError("", "Błąd podczas usuwania metody płatności ze sposobu dostawy");
                }

                _deliveryMethodRepository.SaveDeliveryMethod(deliveryPaymentModificationModel.DeliveryMethod);

                return RedirectToAction(nameof(Index));
            }

            return View(deliveryPaymentModificationModel.DeliveryMethod.DeliveryMethodId);
        }

    }
}