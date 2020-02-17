using System.ComponentModel.DataAnnotations;

namespace TheBookShop.Models.DataModels
{
    public class ProductCategory
    {
        public int ProductCategoryId { get; set; }

        [Display(Name = "Kategoria")]
        [Required(ErrorMessage = "Proszę podać nazwę kategorii.")]
        public string Name { get; set; }
    }
}