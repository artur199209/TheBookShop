using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Infrastructure;
using TheBookShop.Models;
using TheBookShop.Models.ViewModel;

namespace TheBookShop.Controllers
{
    [Route("")]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;

        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        [Route("")]
        [Route("[action]")]
        public ViewResult Index()
        {
            return View();
        }

        [Route("[action]")]
        public ViewResult List(string category, int productPage = 1)
            => View(new ProductsListViewModel
            {
                Products = repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductId)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? repository.Products.Count() 
                        : repository.Products.Count(x => x.Category == category)
                },
                CurrentCategory = category
            });

        [Route("[action]")]
        public ViewResult ProductDetails(int productId)
        {
            var product = repository.Products.FirstOrDefault(x => x.ProductId == productId);
            return View(product);
        }
    }
}