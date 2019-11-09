using System.ComponentModel.DataAnnotations;

namespace TheBookShop.Models.DataModels
{
    public class CreateModel
    {
        [Required(ErrorMessage = "Proszę podać nazwę użytkownika.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Proszę podać adres email.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Proszę podać hasło.")]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Proszę podać hasło jeszcze raz.")]
        [Display(Name = "Potwierdź hasło")]
        [Compare(nameof(Password), ErrorMessage = "Podane hasła różnią się!")]
        public string ConfirmPassword { get; set; }
    }
}