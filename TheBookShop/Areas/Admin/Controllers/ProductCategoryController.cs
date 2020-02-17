using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryController(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            return View(_productCategoryRepository.ProductCategories);
        }

        [Route("[action]")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public IActionResult Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                var productCategory = new ProductCategory { Name = name };
                _productCategoryRepository.SaveCategory(productCategory);

                return RedirectToAction(nameof(Index));
            }

            return View(name);
        }

        [Route("[action]")]
        public IActionResult Edit(int productCategoryId)
        {
            var category = _productCategoryRepository.ProductCategories.FirstOrDefault(x => x.ProductCategoryId == productCategoryId);

            if (category != null)
            {
                return View(category);
            }

            return Edit(productCategoryId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public IActionResult Edit(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                _productCategoryRepository.SaveCategory(model);

                return RedirectToAction(nameof(Index));
            }
            
            return Edit(model.ProductCategoryId);
        }
    }
}