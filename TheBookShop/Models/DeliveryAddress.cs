using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TheBookShop.Models
{
    public class DeliveryAddress
    {
        [BindNever]
        public int DeliveryAddressId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string HomeNumber { get; set; }
    }
}