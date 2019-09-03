﻿namespace TheBookShop.Models
{
    public class Payment
    {
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public string PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}