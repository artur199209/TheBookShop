using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TheBookShop.Models.Repositories;

namespace TheBookShop.Components
{
    public class PaymentDeliveryMethodViewComponent : ViewComponent
    {
        private readonly IDeliveryMethodRepository _deliveryMethodRepository;

        public PaymentDeliveryMethodViewComponent(IDeliveryMethodRepository deliveryMethodRepository)
        {
            _deliveryMethodRepository = deliveryMethodRepository;
        }
        
        public IViewComponentResult Invoke()
        {
            var deliveryAndPaymentMethods = _deliveryMethodRepository.DeliveryMethods.ToList();
            return View(deliveryAndPaymentMethods);
        }
    }
}