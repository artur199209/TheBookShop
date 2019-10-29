﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models.DataModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("")]
        [Route("[action]")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel details)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(details.Email);

                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    SignInResult result =
                        await _signInManager.PasswordSignInAsync(user, details.Password, false, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                ModelState.AddModelError(nameof(LoginModel.Email), "Niepoprawny adres email albo hasło!");
            }

            return View(details);
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