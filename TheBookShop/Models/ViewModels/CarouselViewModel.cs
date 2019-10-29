using System.Collections.Generic;

namespace TheBookShop.Models.ViewModels
{
    public class CarouselViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public string Category { get; set; }
        public int Id { get; set; }
    }
}