using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TheBookShop.Models
{
    public class Order
    {
        [BindNever]
        public int OrderId { get; set; }
        [BindNever]
        public ICollection<CartLine> Lines { get; set; }
        
        public Customer Customer { get; set; }

        [BindNever]
        public bool Shipped { get; set; }
        
        public DeliveryAddress DeliveryAddress { get; set; }
        public bool GiftWrap { get; set; }
    }
}