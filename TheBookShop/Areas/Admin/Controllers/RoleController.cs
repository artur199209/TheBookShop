using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TheBookShop.Areas.Admin.Model;
using TheBookShop.Models.DataModels;

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
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                Log.Information($"Start creating new role with Name {name}...");

                if (!await _roleManager.RoleExistsAsync(name))
                {
                    IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));

                    if (result.Succeeded)
                    {
                        Log.Information($"The role {name} has been created succesfully...");

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
            Log.Information($"Finding role by Id: {id}");

            IdentityRole role = await _roleManager.FindByIdAsync(id);

            var members = new List<AppUser>();
            var nonMembers = new List<AppUser>();

            foreach (var user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> Edit(RoleModificationModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result;
                foreach (string userId in model.IdsToAdd ?? new string [] {})
                {
                    Log.Information($"Finding user by Id: {userId}");

                    AppUser user = await _userManager.FindByIdAsync(userId);

                    if (user != null)
                    {
                        result = await _userManager.AddToRoleAsync(user, model.RoleName);

                        if (!result.Succeeded)
                        {
                            AddErrosFromResult(result);
                        }
                    }
                }

                foreach (string userId in model.IdsToDelete ?? new string[] {})
                {
                    Log.Information($"Finding user by Id: {userId}");
                    AppUser user = await _userManager.FindByIdAsync(userId);

                    if (user != null)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

                        if (!result.Succeeded)
                        {
                            Log.Information($"User {user.Email} has been removed successfully...");
                            AddErrosFromResult(result);
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            return await Edit(model.RoleId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> Delete(string id)
        {
            Log.Information($"Finding role by Id: {id}");
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    Log.Information($"The role {id} has been removed successfully...");
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