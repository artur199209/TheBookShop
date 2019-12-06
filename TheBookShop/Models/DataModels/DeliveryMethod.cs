using System.Collections.Generic;

namespace TheBookShop.Models.DataModels
{
    public class DeliveryMethod
    {
        public int DeliveryMethodId { get; set; }
        public string Name { get; set; }
        public ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
}