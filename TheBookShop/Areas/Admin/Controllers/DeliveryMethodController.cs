using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using System.Linq;
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
        
        [Route("[action]")]
        public IActionResult Edit(int deliveryMethodId)
        {
            var deliveryMethod = _deliveryMethodRepository.DeliveryMethods.FirstOrDefault(x => x.DeliveryMethodId == deliveryMethodId);
            var allPaymentMethods = _paymentMethodRepository.PaymentMethods;
            var excludedIds = deliveryMethod?.PaymentMethods.Select(x => x.PaymentMethodId).ToList();

            var deliveryPaymentViewModel = new DeliveryPaymentViewModel
            {
                DeliveryMethod = deliveryMethod,
                NonPaymentMethods = allPaymentMethods.Where(w => !excludedIds.Contains(w.PaymentMethodId)).ToList()
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
                deliveryPaymentModificationModel.DeliveryMethod.PaymentMethods = _deliveryMethodRepository
                    .DeliveryMethods.FirstOrDefault(x =>
                        x.DeliveryMethodId == deliveryPaymentModificationModel.DeliveryMethod.DeliveryMethodId)
                    ?.PaymentMethods ?? new List<DeliveryPaymentMethod>();

                foreach (int id in deliveryPaymentModificationModel.IdsToAdd ?? new List<int>())
                {
                    var paymentMethod = new DeliveryPaymentMethod
                    {
                        PaymentMethod = _paymentMethodRepository.PaymentMethods.FirstOrDefault(x => x.PaymentMethodId == id)
                    };
                    deliveryPaymentModificationModel.DeliveryMethod.PaymentMethods?.Add(paymentMethod);
                }

                foreach (int id in deliveryPaymentModificationModel.IdsToDelete ?? new List<int>())
                {
                    var index = deliveryPaymentModificationModel.DeliveryMethod.PaymentMethods.ToList().FindIndex(x =>
                        x.PaymentMethodId == id);
                    deliveryPaymentModificationModel.DeliveryMethod.PaymentMethods.RemoveAt(index);
                }

                _deliveryMethodRepository.SaveDeliveryMethod(deliveryPaymentModificationModel.DeliveryMethod);
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            return Edit(deliveryPaymentModificationModel.DeliveryMethod.DeliveryMethodId);
        }
    }
}