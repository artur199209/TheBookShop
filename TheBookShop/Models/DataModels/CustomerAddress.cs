using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TheBookShop.Models.DataModels
{
    public class CustomerAddress
    {
        [BindNever]
        public int CustomerAddressId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string HomeNumber { get; set; }
    }
}