using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;

namespace TheBookShop.Controllers
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly Cart _cart;

        public OrderController(IOrderRepository orderRepository, Cart cartService, IPaymentMethodRepository paymentMethodRepository = null)
        {
            _orderRepository = orderRepository;
            _cart = cartService;
            _paymentMethodRepository = paymentMethodRepository;
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
            Log.Information($"Starting checkout order...");
            order.Lines = _cart.Lines.ToArray();

            if (!_cart.Lines.Any())
            {
                ModelState.AddModelError("", "Twój koszyk jest pusty!");
            }
            
            if (ModelState.IsValid)
            {
                order.DeliveryPaymentMethod.PaymentMethod =
                    _paymentMethodRepository.PaymentMethods.FirstOrDefault(x =>
                        x.PaymentMethodId == order.DeliveryPaymentMethod.PaymentMethodId);
               
                order.OrderGuidId = Guid.NewGuid();
                order.Cost = order.CalculateTotalCosts();
                order.Payment = new Payment
                {
                    Amount = order.CalculateTotalCosts(),
                    Customer = order.Customer,
                    PaymentDate = DateTime.Now
                };
                _orderRepository.SaveOrder(order);
                return RedirectToAction(nameof(Completed), new { orderNumber = order.OrderGuidId });
            }

            return View(order);
        }

        [Route("[action]")]
        public ViewResult LoginOrRegister()
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
            Log.Information($"Getting orders for user {email}...");
            var myOrders = _orderRepository.Orders.Where(x => x.Customer.Email == email);

            return View(myOrders);
        }

        [Route("[action]")]
        public IActionResult OrderDetails(int orderId)
        {
            Log.Information($"Getting order details with Id: {orderId}...");

            var order = _orderRepository.Orders.FirstOrDefault(x => x.OrderId == orderId);

            if (order != null)
            {
                return View(order);
            }

            return RedirectToAction(nameof(MyOrders));
        }
    }
}