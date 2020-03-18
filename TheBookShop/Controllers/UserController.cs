using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TheBookShop.Models.DataModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TheBookShop.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        
        private readonly IPasswordValidator<AppUser> _passwordValidator;
        private readonly IPasswordHasher<AppUser> _passwordHasher;


        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IPasswordValidator<AppUser> passwordValidator, IPasswordHasher<AppUser> passwordHasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordValidator = passwordValidator;
            _passwordHasher = passwordHasher;
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
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Log.Information($"Finding user by email {model.Email}...");
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    SignInResult result =
                        await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    
                    if (result.Succeeded)
                    {
                        Log.Information($"{model.Email} logged in...");
                        return RedirectToAction("Index", "Product");
                    }
                }

                ModelState.AddModelError(nameof(LoginModel.Email), "Niepoprawny adres email albo hasło!");
            }

            return View(model);
        }

        [Route("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [Route("[action]")]
        public async Task<IActionResult> ChangePassword(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                return View(user);
            }

            return RedirectToAction("List", "Product", new { area = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> ChangePassword(string id, string email, string password)
        {
            Log.Information($"Finding user {email}...");
            AppUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult validPassword = null;

                if (!string.IsNullOrEmpty(password))
                {
                    Log.Information($"Password is validating...");
                    validPassword = await _passwordValidator.ValidateAsync(_userManager, user, password);

                    if (validPassword.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPassword);
                    }
                }

                if (validPassword == null ||
                    password != string.Empty && validPassword.Succeeded)
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("List", "Product", new {area = ""});
                    }

                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Nie znaleziono użytkownika.");
            }

            return View(user);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}