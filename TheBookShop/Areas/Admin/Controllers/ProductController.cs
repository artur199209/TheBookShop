using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TheBookShop.Infrastructure;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using TheBookShop.Models.ViewModels;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        public int PageSize = 10;

        public ProductController(IProductRepository repo, IAuthorRepository authorRepository, IProductCategoryRepository productCategoryRepository)
        {
            _repository = repo;
            _authorRepository = authorRepository;
            _productCategoryRepository = productCategoryRepository;
        }
        
        [Route("[action]")]
        public IActionResult Index(int page = 1)
        {
            return View(new ProductsListViewModel
            {
                Products = _repository.Products
                    .OrderBy(p => p.ProductId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems =  _repository.Products.Count()
                }
            });
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

        [Route("[action]")]
        public IActionResult Opinion(int productId, int page = 1)
        {
            var opinions = _repository.Products.FirstOrDefault(x => x.ProductId == productId)?.Opinions;

            return View(new OpinionsListViewModel()
            {
                Opinions = opinions?.Skip((page - 1) * PageSize).Take(PageSize).AsQueryable(),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = opinions?.Count ?? 0
                },
                ProductId = productId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public IActionResult Edit(Product product, IFormCollection formValues)
        {
            if (ModelState.IsValid)
            {
                if (formValues != null)
                {
                    var authorId = Convert.ToInt32(formValues["Author"]);
                    var author = _authorRepository.GetAuthorById(authorId);

                    var categoryId = Convert.ToInt32(formValues["Category"]);
                    var category =
                        _productCategoryRepository.ProductCategories.FirstOrDefault(x =>
                            x.ProductCategoryId == categoryId);
                    IFormFile file = formValues.Files.FirstOrDefault();

                    if (author != null)
                    {
                        product.Author = author;
                    }

                    if (category != null)
                    {
                        product.Category = category;
                    }

                    if (file != null)
                    {
                        product.Image = "\\Images\\" + file.FileName;
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
        [ValidateAntiForgeryToken]
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