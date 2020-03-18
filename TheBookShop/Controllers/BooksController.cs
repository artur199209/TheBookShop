using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using TheBookShop.Models.ViewModels;

namespace TheBookShop.Controllers
{
    [Route("[controller]")]
    public class BooksController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        public int PageSize = 4;

        public BooksController(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        [Route("[action]")]
        public IActionResult Bestsellers()
        {
            Log.Information($"Getting bestsellers books...");

            var shippedItems = _orderRepository.Orders.Where(x => x.Status == Order.OrderStatus.Shipped).ToList();
           
            var productsWithQuantity = shippedItems.SelectMany(x => x.Lines)
                .GroupBy(x => x.Product, (key, group) => new { Product = key, Quantity = group.Sum(x => x.Quantity)})
                .OrderByDescending(c => c.Quantity).Select(x => x.Product).Take(12).ToList();

            return View(productsWithQuantity);
        }

        [Route("[action]")]
        public IActionResult Sales(int page = 1)
        {
            Log.Information($"Getting sales books...");

            var salesProducts = _productRepository.Products
                .Where(x => x.SalesType == Product.SalesTypeEnums.BookSale)
                .OrderBy(p => p.ProductId).ToList();

            return View(new ProductsListViewModel
            {
                Products = salesProducts
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = salesProducts.Count
                }
            });
        }

        [Route("[action]")]
        public IActionResult Previews(int page = 1)
        {
            Log.Information($"Getting previews books...");

            var previewProducts = _productRepository.Products.Where(x => x.SalesType == Product.SalesTypeEnums.BookPreview)
                .OrderBy(p => p.ProductId).ToList();

            return View(new ProductsListViewModel
            {
                Products = previewProducts
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = previewProducts.Count
                }
            });
        }

    }
}