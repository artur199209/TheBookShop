using System.Collections.Generic;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.ViewModels
{
    public class CarouselViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public string Category { get; set; }
        public int Id { get; set; }
    }
}