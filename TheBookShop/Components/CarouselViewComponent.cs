using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.ViewModels;

namespace TheBookShop.Components
{
    public class CarouselViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<Product> products, string category)
        {
            var carouselComponentViewModel = new CarouselComponentViewModel
            {
                Products = products,
                Category = category
            };
            return View(carouselComponentViewModel);
        }
    }
}