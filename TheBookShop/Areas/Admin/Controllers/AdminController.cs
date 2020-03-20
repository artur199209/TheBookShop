using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBookShop.Models;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]")]
    [Authorize(Roles = "Administratorzy")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly IOrderRepository _orderRepository;

        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IOrderRepository orderRepository = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _orderRepository = orderRepository;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            var shippedItems = _orderRepository.Orders.Where(x => x.Status == Order.OrderStatus.Shipped).ToList();
            
            var categoryWithQuantity = shippedItems.SelectMany(x => x.Lines)
                .GroupBy(x => x.Product.Category,
                    (key, group) => new DataPoint {Label = key.Name, Y = 100 * group.Sum(x => x.Quantity)/shippedItems.Sum(p => p.Lines.Sum(c => c.Quantity))}).ToList();

            var bestsellersProducts = shippedItems.SelectMany(x => x.Lines)
                .GroupBy(x => x.Product, (key, group) => new { Product = key, Quantity = group.Sum(x => x.Quantity) })
                .OrderByDescending(c => c.Quantity).Select(x => new DataPoint
                {
                    Label = x.Product.Title,
                    Y = x.Quantity
                }).ToList();

            var chartsData = new List<List<DataPoint>> {categoryWithQuantity, bestsellersProducts};

            return View(chartsData);
        }

        [Route("")]
        [Route("[action]")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [Route("[action]")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                Log.Information($"Finding user by email: {loginModel.Email}...");

                var user = await _userManager.FindByEmailAsync(loginModel.Email);

                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    SignInResult result =
                        await _signInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, false);

                    if (result.Succeeded)
                    {
                        Log.Information("Admin logged in successfully...");
                        return RedirectToAction(nameof(Index));
                    }
                }

                ModelState.AddModelError(nameof(LoginModel.Email), "Niepoprawny adres email albo hasło!");
            }

            return View(loginModel);
        }

        [Route("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }
     
        [AllowAnonymous]
        [Route("[action]")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}