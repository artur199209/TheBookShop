using System;

namespace TheBookShop.Models.DataModels
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public Customer Customer { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}