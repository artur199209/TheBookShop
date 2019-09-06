using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Infrastructure;
using TheBookShop.Models;

namespace TheBookShop.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }

        public IActionResult Index()
        {
            return View(repository.Products);
        }

        public IActionResult Create()
        {
            return View(nameof(Edit), new Product());
        }

        public IActionResult Edit(int productId)
        {
            var product = repository.Products.FirstOrDefault(o => o.ProductId == productId);

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                IFormFile file = HttpContext.Request.Form.Files.FirstOrDefault();
                product.Image = "\\Images\\" + file?.FileName;
                FileHelper.CopyImageFile(file);
                repository.SaveProduct(product);
                TempData["message"] = $"{product.Title} has been saved";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(int productId)
        {
            var deletedProduct = repository.DeleteProduct(productId);

            if (deletedProduct != null)
            {
                TempData["message"] = $"{deletedProduct.Title} was deleted";
            }
            return View(nameof(Index));
        }
    }
}