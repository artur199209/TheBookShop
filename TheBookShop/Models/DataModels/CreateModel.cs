using System.ComponentModel.DataAnnotations;

namespace TheBookShop.Models.DataModels
{
    public class CreateModel
    {
        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
    }
}