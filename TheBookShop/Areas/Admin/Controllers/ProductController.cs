using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Infrastructure;
using TheBookShop.Models;
using TheBookShop.Models.Repositories;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IAuthorRepository _authorRepository;

        public ProductController(IProductRepository repo, IAuthorRepository authorRepository)
        {
            _repository = repo;
            _authorRepository = authorRepository;
        }
        
        [Route("[action]")]
        public IActionResult Index()
        {
            return View(_repository.Products);
        }

        [Route("[action]")]
        public IActionResult Create()
        {
            return View(nameof(Edit), new Product());
        }

        [Route("[action]")]
        public IActionResult Edit(int productId)
        {
            var product = _repository.Products.FirstOrDefault(o => o.ProductId == productId);

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

                _repository.SaveProduct(product);
                TempData["message"] = $"{product.Title} has been saved";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Delete(int productId)
        {
            var deletedProduct = _repository.DeleteProduct(productId);

            if (deletedProduct != null)
            {
                TempData["message"] = $"{deletedProduct.Title} was deleted";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}