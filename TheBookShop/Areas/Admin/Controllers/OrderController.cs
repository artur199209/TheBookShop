using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models.Repositories;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            ViewData["Status"] = "Wszystkie";
            return View(_orderRepository.Orders);
        }

        [Route("[action]")]
        public IActionResult Completed()
        {
            ViewData["Status"] = "Zakończone";
            return View(nameof(Index), _orderRepository.Orders.Where(x => x.Shipped));
        }

        [Route("[action]")]
        public IActionResult NotCompleted()
        {
            ViewData["Status"] = "Realizowane";
            return View(nameof(Index), _orderRepository.Orders.Where(x => !x.Shipped));
        }

        [Route("[action]")]
        public IActionResult Edit(int orderId)
        {
            var order = _orderRepository.Orders.FirstOrDefault(x => x.OrderId == orderId);

            if (order != null)
            {
                return View(order);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public IActionResult Edit(bool shipped, int orderId)
        {
            var order = _orderRepository.Orders.FirstOrDefault(o => o.OrderId == orderId);

            if (order != null)
            {
                order.Shipped = shipped;
                _orderRepository.SaveOrder(order);
            }

            return RedirectToAction(nameof(Index));
        }

        [Route("[action]")]
        public IActionResult Details(int orderId)
        {
            var order = _orderRepository.Orders.FirstOrDefault(x => x.OrderId == orderId);

            if (order != null)
            {
                return View(order);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}