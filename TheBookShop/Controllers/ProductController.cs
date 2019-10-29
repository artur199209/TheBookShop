using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

        public ProductController(IProductRepository repo)
        {
            _repository = repo;
        }

        [Route("")]
        [Route("[action]")]
        public ViewResult Index()
        {
            return View();
        }

        [Route("[action]")]
        public ViewResult List(string category, int page = 1)
            => View(new ProductsListViewModel
            {
                Products = _repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? _repository.Products.Count() 
                        : _repository.Products.Count(x => x.Category == category)
                },
                CurrentCategory = category
            });

        [Route("[action]")]
        public ViewResult ProductDetails(int productId)
        {
            var product = _repository.Products.FirstOrDefault(x => x.ProductId == productId);
            return View(product);
        }
    }
}