using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.ViewModels;

namespace TheBookShop.Components
{
    public class CarouselViewComponent : ViewComponent
    {
        public CarouselViewComponent()
        {
        }

        public IViewComponentResult Invoke(IEnumerable<Product> products, string category, int id)
        {
            var p = new CarouselViewModel();
            p.ProductsInThePromotion = products;
            p.Category = category;
            p.Id = id;
            return View(p);
        }
    }
}