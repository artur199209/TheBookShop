using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TheBookShop.Models.DataModels
{
    public class Order
    {
        [BindNever]
        public int OrderId { get; set; }
        [BindNever]
        public ICollection<CartLine> Lines { get; set; }
        public Guid OrderGuidId { get; set; }
        public string TrackingNumber { get; set; }
        public Customer Customer { get; set; }
        public bool Shipped { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
        public Payment Payment { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public bool GiftWrap { get; set; }
        
        public decimal CalculateTotalCosts()
        {
            var cartCost = Lines.Sum(x => x.Quantity * x.Product.Price);
            var deliveryCost = PaymentMethod.Price;

            return cartCost + deliveryCost;
        }
    }
}