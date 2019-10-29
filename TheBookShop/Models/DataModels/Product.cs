using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        
        public string Image { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Proszę podać opis")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Proszę podać cenę")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Proszę podać kategorię")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Proszę podać podkategorię")]
        public string Subcategory { get; set; }
        
        public Author Author { get; set; }

        [Required(ErrorMessage = "Proszę podać liczbę stron")]
        public int NumberOfPages { get; set; }

        [Required(ErrorMessage = "Proszę podać rodzaj okładki")]
        public string Cover { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę wydawnictwa")]
        public string PublishingHouse { get; set; }

        [Required(ErrorMessage = "Proszę podać liczbę dostępnych produktów")]
        public int QuantityInStock { get; set; }

        public ICollection<Opinion> Opinions { get; set; }
    }
}