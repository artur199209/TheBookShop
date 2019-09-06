using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Infrastructure;
using TheBookShop.Models;
using TheBookShop.Models.ViewModel;

namespace TheBookShop.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;

        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View();
        }

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

        public ViewResult ProductDetails(int productId)
        {
            var product = repository.Products.FirstOrDefault(x => x.ProductId == productId);
            return View(product);
        }
    }
}