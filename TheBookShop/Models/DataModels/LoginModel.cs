using System.ComponentModel.DataAnnotations;

namespace TheBookShop.Models.DataModels
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Display(Name = "Hasło")]
        [Required]
        public string Password { get; set; }
    }
}