using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TheBookShop.Areas.Admin.Model;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using TheBookShop.Models.ViewModels;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [Authorize(Roles = "Administratorzy")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        public int PageSize = 10;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [Route("[action]")]
        public IActionResult Index(int page = 1)
        {
            Log.Information("Getting all orders...");
            ViewData["Status"] = "Wszystkie";

            return View(new OrderListViewModel
            {
                Orders = _orderRepository.Orders.OrderBy(x => x.OrderId).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _orderRepository.Orders.Count()
                }

            });
        }

        [Route("[action]")]
        public IActionResult Completed(int page = 1)
        {
            Log.Information("Getting completed orders...");
            ViewData["Status"] = "Zakończone";
            return View(nameof(Index),
                new OrderListViewModel
                {
                    Orders = _orderRepository.Orders.Where(x => x.Status == Order.OrderStatus.Shipped)
                        .OrderBy(x => x.OrderId).Skip((page - 1) * PageSize).Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = _orderRepository.Orders.Count()
                    }
                });
        }

        [Route("[action]")]
        public IActionResult NotCompleted(int page = 1)
        {
            Log.Information("Getting not completed orders...");
            ViewData["Status"] = "Realizowane";
            return View(nameof(Index),
                new OrderListViewModel
                {
                    Orders = _orderRepository.Orders.Where(x => x.Status != Order.OrderStatus.Shipped)
                        .OrderBy(x => x.OrderId).Skip((page - 1) * PageSize).Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = _orderRepository.Orders.Count()
                    }
                });
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
        public IActionResult Edit(Order.OrderStatus status, int orderId)
        {
            var order = _orderRepository.Orders.FirstOrDefault(o => o.OrderId == orderId);

            if (order != null)
            {
                order.Status = status;
                _orderRepository.SaveOrder(order);
            }

            return RedirectToAction(nameof(Index));
        }

        [Route("[action]")]
        public IActionResult AddTrackingNumber(int orderId)
        {
            return View(orderId);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddTrackingNumber([Required] string trackingNumber, int orderId)
        {
            if (ModelState.IsValid)
            {
                var order = _orderRepository.Orders.FirstOrDefault(o => o.OrderId == orderId);

                if (order != null)
                {
                    order.TrackingNumber = trackingNumber;
                    _orderRepository.SaveOrder(order);
                    return RedirectToAction(nameof(Index));
                }
            }

            return View();
        }

        [Route("[action]")]
        public IActionResult Details(int orderId)
        {
            Log.Information($"Getting details for order with Id: {orderId}");

            var order = _orderRepository.Orders.FirstOrDefault(x => x.OrderId == orderId);

            if (order != null)
            {
                return View(order);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}