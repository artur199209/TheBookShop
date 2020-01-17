using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TheBookShop.Models.DataModels
{
    public class Order
    {
        [BindNever]
        public int OrderId { get; set; }
        [BindNever]
        public ICollection<CartLine> Lines { get; set; }
        [BindNever]
        public Guid OrderGuidId { get; set; }
        public string TrackingNumber { get; set; }
        public Customer Customer { get; set; }
        public OrderStatus Status { get; set; }
        public bool Shipped { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
        public Payment Payment { get; set; }
        public DeliveryPaymentMethod DeliveryPaymentMethod { get; set; }
        public bool GiftWrap { get; set; }

        public enum OrderStatus
        {
            [Description("Nowe zamówienie")]
            New = 1,
            [Description("W przygotowaniu")]
            InProgress = 2,
            [Description("Gotowe do wysłania")]
            PreparedForShip = 3,
            [Description("Wysłane")]
            Shipped = 4
        }
        //public decimal CalculateTotalCosts()
        //{
        //    var cartCost = Lines.Sum(x => x.Quantity * x.Product.Price);
        //    var deliveryCost = PaymentMethod.Price;

        //    return cartCost + deliveryCost;
        //}
    }
}