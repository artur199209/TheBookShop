using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using TheBookShop.Infrastructure;
using TheBookShop.Models;
using TheBookShop.Models.ViewModel;

namespace TheBookShop.Controllers
{
    [Route("[controller]")]
    public class CartController : Controller
    {
        private IProductRepository repository;
        private Cart cart;

        public CartController(IProductRepository repo, Cart cartService)
        {
            repository = repo;
            cart = cartService;
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
            Product product = repository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
                SaveCart(cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        [Route("[action]")]
        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                cart.RemoveLine(product);
                SaveCart(cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        [Route("[action]")]
        public RedirectToActionResult DecreaseProductCount(int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                cart.DecreaseProductCount(product);
                SaveCart(cart);
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