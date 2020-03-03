using Microsoft.AspNetCore.Mvc;

namespace TheBookShop.Tests.Helper
{
    public static class CastHelper
    {
        private static T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

        private static string GetActionName(IActionResult result)
        {
            return (result as RedirectToActionResult)?.ActionName;
        }
    }
}