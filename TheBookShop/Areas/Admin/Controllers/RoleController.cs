using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            return View(_roleManager.Roles);
        }

        [Route("[action]")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                if (!await _roleManager.RoleExistsAsync(name))
                {
                    IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    AddErrosFromResult(result);
                }
            }

            return View(name);
        }

        [Route("[action]")]
        public async Task<IActionResult> Edit(string id)
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                AddErrosFromResult(result);
            }

            return View(nameof(Index), _roleManager.Roles);
        }

        private void AddErrosFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}