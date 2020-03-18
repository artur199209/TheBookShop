using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace TheBookShop.Tests.Helper
{
    public static class CastHelper
    {
        public static T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

        public static T GetViewComponentModel<T>(IViewComponentResult result) where T : class
        {
            return (result as ViewViewComponentResult)?.ViewData.Model as T;
        }

        public static string GetComponentViewName(IViewComponentResult result)
        {
            return (result as ViewViewComponentResult)?.ViewName;
        }

        public static string GetActionName(IActionResult result)
        {
            return (result as RedirectToActionResult)?.ActionName;
        }

        public static string GetViewName(IActionResult result)
        {
            return (result as ViewResult)?.ViewName;
        }
    }
}