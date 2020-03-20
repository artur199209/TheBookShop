using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TheBookShop.Areas.Admin.Model;
using TheBookShop.Models.Repositories;
using TheBookShop.Models.ViewModels;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [Authorize(Roles = "Administratorzy")]
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        public int PageSize = 10;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [Route("[action]")]
        public IActionResult Index(int page = 1)
        {
            Log.Information("Getting all payments...");

            return View(new PaymentListViewModel
            {
                Payments = _paymentRepository.Payments.OrderBy(p => p.PaymentId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _paymentRepository.Payments.Count()
                }
            });
        }
    }
}