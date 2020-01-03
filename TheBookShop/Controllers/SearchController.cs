using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBookShop.Models.Repositories;
using TheBookShop.Models.ViewModels;

namespace TheBookShop.Controllers
{
    [Route("[controller]")]
    public class SearchController : Controller
    {
        private readonly IProductRepository _productRepository;
        public int PageSize = 4;

        public SearchController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SearchItems(string searchString, int page = 1)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var products = await _productRepository.Products.Where(x => x.Title.Contains(searchString))
                    .OrderBy(p => p.ProductId)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize).ToListAsync();

                var productListViewModel = new ProductsListViewModel
                {
                    Products = products,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = products.Count
                    },
                };

                return View(productListViewModel);
            }

            return View();
        }
    }
}