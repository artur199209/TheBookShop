using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;

namespace TheBookShop.Controllers
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly Cart _cart;

        public OrderController(IOrderRepository orderRepository, Cart cartService)
        {
            _orderRepository = orderRepository;
            _cart = cartService;
        }

        [Route("[action]")]
        public ViewResult Checkout()
        {
            return View(new Order()
            {
                Lines = _cart.Lines.ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public IActionResult Checkout(Order order)
        {
            order.Lines = _cart.Lines.ToArray();

            if (!_cart.Lines.Any())
            {
                ModelState.AddModelError("", "Twój koszyk jest pusty!");
            }

            if (ModelState.IsValid)
            {
                _orderRepository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }

            return View(order);
        }

        [Route("[action]")]
        public ViewResult Test()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(nameof(Checkout));
            }

            return View();
        }

        [Route("[action]")]
        public ViewResult Completed()
        {
            _cart.Clear();
            return View();
        }
    }
}