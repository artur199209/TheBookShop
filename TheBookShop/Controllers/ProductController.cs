using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TheBookShop.Models.Repositories;
using TheBookShop.Models.ViewModels;

namespace TheBookShop.Controllers
{
    [Route("")]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;
        public int PageSize = 4;
        public int NewProductsCount = 12;

        public ProductController(IProductRepository repo)
        {
            _repository = repo;
        }

        [Route("")]
        [Route("[action]")]
        public ViewResult Index()
        {
            var carouselViewModels = new List<CarouselViewModel>
            {
                new CarouselViewModel()
                {
                    Category = "NOWOŚCI",
                    Products = _repository.Products.OrderByDescending(x => x.ProductId).Take(NewProductsCount)
                },
                new CarouselViewModel()
                {
                    Category = "PROMOCJE",
                    Products = _repository.Products.Where(x => x.IsProductInPromotion),
                },
                new CarouselViewModel()
                {
                    Category = "BESTSELLERY",
                    Products = _repository.Products.Where(x => x.IsProductInPromotion),
                }
            };

            return View(carouselViewModels);
        }

        [Route("[action]")]
        public ViewResult List(string category, int page = 1)
            => View(new ProductsListViewModel
            {
                Products = _repository.Products
                    .Where(p => category == null || p.Category.Name == category)
                    .OrderBy(p => p.ProductId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? _repository.Products.Count() 
                        : _repository.Products.Count(x => x.Category.Name == category)
                },
                CurrentCategory = category
            });

        [Route("[action]")]
        public ViewResult ProductDetails(int productId)
        {
            Log.Information($"Getting product details with Id: {productId}...");
            var product = _repository.Products.FirstOrDefault(x => x.ProductId == productId);
            return View(product);
        }
    }
}