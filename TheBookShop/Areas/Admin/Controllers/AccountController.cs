using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TheBookShop.Areas.Admin.Model;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.ViewModels;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserValidator<AppUser> _userValidator;
        private readonly IPasswordValidator<AppUser> _passwordValidator;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        public int PageSize = 4;

        public AccountController(UserManager<AppUser> userManager, IUserValidator<AppUser> userValidator,
            IPasswordValidator<AppUser> passwordValidator, IPasswordHasher<AppUser> passwordHasher)
        {
            _userManager = userManager;
            _userValidator = userValidator;
            _passwordValidator = passwordValidator;
            _passwordHasher = passwordHasher;
        }

        [Route("")]
        [Route("[action]")]
        public IActionResult Index(int page = 1)
        {
            return View(new AccountListViewModel
                {
                    Accounts = _userManager.Users.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = _userManager.Users.Count()
                    }
                });
        }

        [Route("[action]")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> Create(CreateModel model)
        {
            Log.Information("Start creating a new account...");
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    Log.Information($"The account {model.Name} has been created successfully...");
                    return RedirectToAction(nameof(Index));
                }

                AddErrorsFromResult(result);
            }

            return View(model);
        }

        [Route("[action]")]
        public async Task<IActionResult> Edit(string id)
        {
            Log.Information($"Getting user by Id: {id}...");
            AppUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                return View(user);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            Log.Information($"Start editing account with email {email}...");
            AppUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                user.Email = email;
                Log.Information($"{email} is validating...");
                IdentityResult validEmail = await _userValidator.ValidateAsync(_userManager, user);

                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }

                IdentityResult validPassword = null;

                if (!string.IsNullOrEmpty(password))
                {
                    Log.Information($"{password} is validating...");
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
                        Log.Information($"The account {email} has been updated successfully...");
                        return RedirectToAction(nameof(Index));
                    }

                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Nie znaleziono użytkownika...");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> Delete(string id)
        {
            Log.Information($"Start deleting the account with Id: {id}...");
            AppUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    Log.Information($"User {user.Email} has been deleted successfully...");
                    return RedirectToAction(nameof(Index));
                }

                AddErrorsFromResult(result);
            }
            else
            {
                ModelState.AddModelError("", "Nie znaleziono użytkownika.");
            }

            return View(nameof(Index));
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