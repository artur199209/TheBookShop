﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace TheBookShop.Models.DataModels
{
    public class Author
    {
        public int AuthorId { get; set; }

        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Proszę podać imię autora")]
        public string Name { get; set; }
        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Proszę podać nazwisko autora")]
        public string Surname { get; set; }
        [Display(Name = "Notka")]
        [Required(ErrorMessage = "Proszę podać notatki o autorze")]
        public string Notes { get; set; }
        [JsonIgnore]
        [BindNever]
        public ICollection<Product> Products { get; set; }
    }
}