using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TheBookShop.Models.DataModels
{
    public class Order
    {
        [BindNever]
        public int OrderId { get; set; }
        [BindNever]
        public ICollection<CartLine> Lines { get; set; }
        
        public Customer Customer { get; set; }
      
        public bool Shipped { get; set; }
        
        public DeliveryAddress DeliveryAddress { get; set; }
        public Payment Payment { get; set; }
        public bool GiftWrap { get; set; }

        public enum OrderStatus
        {
            New,
            InProgress,
            Completed
        }
    }
}