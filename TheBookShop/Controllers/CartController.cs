using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TheBookShop.Infrastructure;
using TheBookShop.Models;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using TheBookShop.Models.ViewModels;

namespace TheBookShop.Controllers
{
    [Route("[controller]")]
    public class CartController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly Cart _cart;

        public CartController(IProductRepository repo, Cart cartService)
        {
            _repository = repo;
            _cart = cartService;
        }

        [Route("[action]")]
        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel()
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        [Route("[action]")]
        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                _cart.AddItem(product, 1);
                SaveCart(_cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        [Route("[action]")]
        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                _cart.RemoveLine(product);
                SaveCart(_cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        [Route("[action]")]
        public RedirectToActionResult DecreaseProductCount(int productId, string returnUrl)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                _cart.DecreaseProductCount(product);
                SaveCart(_cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }

        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
    }
}