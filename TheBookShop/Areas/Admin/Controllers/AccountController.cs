using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserValidator<IdentityUser> _userValidator;
        private readonly IPasswordValidator<IdentityUser> _passwordValidator;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;

        public AccountController(UserManager<IdentityUser> userManager, IUserValidator<IdentityUser> userValidator,
            IPasswordValidator<IdentityUser> passwordValidator, IPasswordHasher<IdentityUser> passwordHasher)
        {
            _userManager = userManager;
            _userValidator = userValidator;
            _passwordValidator = passwordValidator;
            _passwordHasher = passwordHasher;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

        [Route("[action]")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                AddErrorsFromResult(result);
            }

            return View(model);
        }

        [Route("[action]")]
        public async Task<IActionResult> Edit(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                return View(user);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail = await _userValidator.ValidateAsync(_userManager, user);

                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }

                IdentityResult validPassword = null;

                if (!string.IsNullOrEmpty(password))
                {
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

                if (validEmail.Succeeded && validPassword == null ||
                    validEmail.Succeeded && password != string.Empty && validPassword.Succeeded)
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
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

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                AddErrorsFromResult(result);
            }
            else
            {
                ModelState.AddModelError("", "Nie znaleziono użytkownika.");
            }

            return View(nameof(Index), _userManager.Users);
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