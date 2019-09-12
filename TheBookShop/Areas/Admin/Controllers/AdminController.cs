using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Infrastructure;
using TheBookShop.Models;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly IProductRepository repository;
        private readonly IAuthorRepository _authorRepository;

        public AdminController(IProductRepository repo, IAuthorRepository authorRepository)
        {
            repository = repo;
            _authorRepository = authorRepository;
        }

        [Route("")]
        [Route("[action]")]
        public IActionResult Index()
        {
            return View(repository.Products);
        }

        [Route("[action]")]
        public IActionResult Create()
        {
            return View(nameof(Edit), new Product());
        }

        [Route("[action]")]
        public IActionResult Edit(int productId)
        {
            var product = repository.Products.FirstOrDefault(o => o.ProductId == productId);

            return View(product);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Edit(Product product, IFormCollection formValues)
        {
            if (ModelState.IsValid)
            {
                if (formValues != null)
                {
                    var authorId = Convert.ToInt32(formValues["Author"]);
                    var author = _authorRepository.GetAuthorById(authorId);
                    IFormFile file = formValues.Files.FirstOrDefault();

                    if (author != null)
                    {
                        product.Author = author;
                    }

                    if (file != null)
                    {
                        product.Image = "\\Images\\" + file?.FileName;
                        FileHelper.CopyImageFile(file);
                    }
                }

                repository.SaveProduct(product);
                TempData["message"] = $"{product.Title} has been saved";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [HttpPost]
        [Route("[action]")]
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