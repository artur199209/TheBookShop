using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models;

namespace TheBookShop.Components
{
    public class LoginRegisterViewComponent : ViewComponent
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public LoginRegisterViewComponent(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (_signInManager.IsSignedIn(User as ClaimsPrincipal))
            {
                return View("SignedIn", await _userManager.GetUserAsync(User as ClaimsPrincipal));
            }

            return View("SignedOut");
        }
    }
}