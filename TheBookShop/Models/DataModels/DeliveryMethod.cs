using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TheBookShop.Models.DataModels
{
    public class DeliveryMethod
    {
        public int DeliveryMethodId { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę sposobu dostawy.")]
        public string Name { get; set; }

        [BindNever]
        public ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
}