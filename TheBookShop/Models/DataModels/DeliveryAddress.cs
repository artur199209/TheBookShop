using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TheBookShop.Models.DataModels
{
    public class DeliveryAddress
    {
        [BindNever]
        public int DeliveryAddressId { get; set; }
        [Display(Name = "Kraj")]
        public string Country { get; set; }
        
        [Display(Name = "Miasto")]
        public string City { get; set; }
        
        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }
        
        [Display(Name = "Nr domu/mieszkania")]
        public string HomeNumber { get; set; }
    }
}