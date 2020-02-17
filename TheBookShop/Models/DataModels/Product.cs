using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Display(Name = "Obraz")]
        public string Image { get; set; }
        [Display(Name = "Tytuł")]
        [Required(ErrorMessage = "Proszę podać nazwę")]
        public string Title { get; set; }
        [Display(Name = "Opis")]
        [Required(ErrorMessage = "Proszę podać opis")]
        public string Description { get; set; }
        [Display(Name = "Cena")]
        [Required(ErrorMessage = "Proszę podać cenę")]
        public decimal Price { get; set; }
        [Display(Name = "Kategoria")]
        [Required(ErrorMessage = "Proszę podać kategorię")]
        public string Category { get; set; }
        [Display(Name = "Podkategoria")]
        [Required(ErrorMessage = "Proszę podać podkategorię")]
        public string Subcategory { get; set; }
        [Display(Name = "Autor")]
        public Author Author { get; set; }
        [Display(Name = "Liczba stron")]
        [Required(ErrorMessage = "Proszę podać liczbę stron")]
        public int NumberOfPages { get; set; }
        [Display(Name = "Okładka")]
        [Required(ErrorMessage = "Proszę podać rodzaj okładki")]
        public string Cover { get; set; }
        [Display(Name = "Wydawnictwo")]
        [Required(ErrorMessage = "Proszę podać nazwę wydawnictwa")]
        public string PublishingHouse { get; set; }
        [Display(Name = "Dostępna liczba")]
        [Required(ErrorMessage = "Proszę podać liczbę dostępnych produktów")]
        public int QuantityInStock { get; set; }

        public ICollection<Opinion> Opinions { get; set; }
    }
}