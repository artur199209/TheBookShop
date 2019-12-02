using System.Collections.Generic;

namespace TheBookShop.Models.DataModels
{
    public class DeliveryMethod
    {
        public int DeliveryMethodId { get; set; }
        public string Name { get; set; }
        public string NameWithoutSpace { get; set; }
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }
    }
}
