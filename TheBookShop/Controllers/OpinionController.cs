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
        public IActionResult SaveOpinion(int productId, string name, string opinionDescription)
        {
            if (ModelState.IsValid)
            {
                var product = _productRepository.Products.FirstOrDefault(x => x.ProductId == productId);
                var opinion = new Opinion
                {
                    Product = product,
                    Name = name,
                    OpinionDescription = opinionDescription
                };

                _opinionRepository.SaveOpinion(opinion);
                return RedirectToAction("List", "Product");
            }

            return WriteOpinion(productId);
        }
    }
}