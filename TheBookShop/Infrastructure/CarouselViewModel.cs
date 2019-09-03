using System.Collections.Generic;
using TheBookShop.Models;

namespace TheBookShop.Infrastructure
{
    public class CarouselViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public string Category { get; set; }
        public int Id { get; set; }
    }
}