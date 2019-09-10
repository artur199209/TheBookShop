using System.Collections.Generic;

namespace TheBookShop.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public CustomerAddress CustomerAddress { get; set; }
        
        public ICollection<Order> Orders { get; set; }
    }
}