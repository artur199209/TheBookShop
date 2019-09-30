using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models;

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
            return View(_orderRepository.Orders);
        }
    }
}