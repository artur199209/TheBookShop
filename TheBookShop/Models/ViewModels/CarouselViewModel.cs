﻿using System.Collections.Generic;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.ViewModels
{
    public class CarouselViewModel
    {
        public IEnumerable<Product> NewProducts { get; set; }
        public IEnumerable<Product> ProductsInThePromotion { get; set; }
        public string Category { get; set; }
    }
}