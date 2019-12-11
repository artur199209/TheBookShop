using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TheBookShop.Models.DataModels
{
    public class DeliveryMethod
    {
        public int DeliveryMethodId { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę sposobu dostawy.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [BindNever]
        public IList<DeliveryPaymentMethod> PaymentMethods { get; set; } = new List<DeliveryPaymentMethod>();
    }
}