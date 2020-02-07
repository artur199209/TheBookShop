using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TheBookShop.Models;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;

namespace TheBookShop.Controllers
{
    [Route("[controller]")]
    public class OpinionController : Controller
    {
        private readonly IOpinionRepository _opinionRepository;
        private readonly IProductRepository _productRepository;

        public OpinionController(IOpinionRepository opinionRepository, IProductRepository productRepository)
        {
            _opinionRepository = opinionRepository;
            _productRepository = productRepository;
        }

        [Route("[action]")]
        public IActionResult WriteOpinion(int productId)
        {
            return View(new Opinion
            {
                Product = new Product { ProductId = productId }
            });
        }

        [HttpPost]
        [Route("action")]
        [ValidateAntiForgeryToken]
        public IActionResult SaveOpinion(Opinion opinion, int productId)
        {
            if (ModelState.IsValid)
            {
                var product = _productRepository.Products.FirstOrDefault(x => x.ProductId == productId);
                opinion.Product = product;

                _opinionRepository.SaveOpinion(opinion);
                return RedirectToAction("List", "Product");
            }

            return WriteOpinion(productId);
        }
    }
}