using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheBookShop.Models.DataModels
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
        public ProductCategory Category { get; set; }
        [Display(Name = "Typ sprzedaży")]
        [Required(ErrorMessage = "Proszę podać typ sprzedaży")]
        public SalesTypeEnums SalesType { get; set; }
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

        public ICollection<Opinion> Opinions { get; set; }

        public enum SalesTypeEnums
        {
            [Description("Zapowiedź")]
            BookPreview,
            [Description("Sprzedaż")]
            Book,
            [Description("Wyprzedaż")]
            BookSale
        }
    }
}