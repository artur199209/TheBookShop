using System.Collections.Generic;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.ViewModels
{
    public class CarouselComponentViewModel
    {
        public IEnumerable<Product> Products { get; set; }

        public string Category { get; set; }
    }
}