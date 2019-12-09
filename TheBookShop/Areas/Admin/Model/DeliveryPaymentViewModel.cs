using System.Collections.Generic;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Areas.Admin.Model
{
    public class DeliveryPaymentViewModel
    {
        public DeliveryMethod DeliveryMethod { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; }
    }
}
