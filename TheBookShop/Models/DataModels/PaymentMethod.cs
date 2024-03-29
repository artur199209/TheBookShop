﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TheBookShop.Models.DataModels
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Proszę podać koszt dostawy.")]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        [BindNever]
        public IList<DeliveryPaymentMethod> DeliveryMethods { get; set; } = new List<DeliveryPaymentMethod>();
    }
}