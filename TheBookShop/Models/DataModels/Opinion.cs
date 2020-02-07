using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace TheBookShop.Models.DataModels
{
    public class Opinion
    {
        [BindNever]
        public int OpinionId { get; set; }
        [BindNever]
        public Product Product { get; set; }
        [Required(ErrorMessage = "Proszę podać swoje imię.")]
        [Display(Name = "Imię")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Proszę napisać opinię.")]
        [Display(Name = "Opinia")]
        public string OpinionDescription { get; set; }
        [Display(Name = "Ocena")]
        [HiddenInput]
        public int Rating { get; set; }
    }
}