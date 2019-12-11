using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheBookShop.Models.DataModels
{
    public class DeliveryPaymentMethod
    {
        [ForeignKey(nameof(DeliveryMethodId))]
        public int DeliveryMethodId { get; set; }
        [ForeignKey(nameof(PaymentMethodId))]
        public int PaymentMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
