using System;
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
                order.OrderGuidId = Guid.NewGuid();
                _orderRepository.SaveOrder(order);
                return RedirectToAction(nameof(Completed), new { orderNumber = order.OrderGuidId });
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
        public ViewResult Completed(Guid orderNumber)
        {
            _cart.Clear();
            return View(orderNumber);
        }

        [Route("[action]")]
        public IActionResult MyOrders(string email)
        {
            var myOrders = _orderRepository.Orders.Where(x => x.Customer.Email == email);

            return View(myOrders);
        }

        [Route("[action]")]
        public IActionResult OrderDetails(int orderId)
        {
            var order = _orderRepository.Orders.FirstOrDefault(x => x.OrderId == orderId);

            if (order != null)
            {
                return View(order);
            }

            return RedirectToAction(nameof(MyOrders));
        }
    }
}