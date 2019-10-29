using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.ViewModels;

namespace TheBookShop.Components
{
    public class CarouselViewComponent : ViewComponent
    {
        private Cart cart;

        public CarouselViewComponent(Cart cartService)
        {
            cart = cartService;
        }

        public IViewComponentResult Invoke(string category, int id)
        {
            var p = new CarouselViewModel();
            p.Category = category;
            p.Id = id;
            return View(p);
        }
    }
}