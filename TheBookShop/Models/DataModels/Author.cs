using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TheBookShop.Models.DataModels
{
    public class Author
    {
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Proszę podać imię autora")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Proszę podać nazwisko autora")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Proszę podać notatki o autorze")]
        public string Notes { get; set; }
        [BindNever]
        public ICollection<Product> Products { get; set; }
    }
}