using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheBookShop.Models.DataModels
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Proszę podać imię.")]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwisko.")]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Proszę podać numer telefonu.")]
        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Proszę podać adres email.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public CustomerAddress CustomerAddress { get; set; }
        
        public ICollection<Order> Orders { get; set; }
    }
}