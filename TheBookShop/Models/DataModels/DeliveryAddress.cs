using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TheBookShop.Models.DataModels
{
    public class DeliveryAddress
    {
        [BindNever]
        public int DeliveryAddressId { get; set; }
        [Required(ErrorMessage = "Proszę podać kraj.")]
        [Display(Name = "Kraj")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Proszę podać miasto.")]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required(ErrorMessage = "Proszę podać kod pocztowy.")]
        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Proszę podać numer domu/mieszkania.")]
        [Display(Name = "Nr domu/mieszkania")]
        public string HomeNumber { get; set; }
    }
}