using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models;

namespace TheBookShop.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class AuthorController : Controller
    {
        private IAuthorRepository _authorRepository;

        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        
        [Route("[action]")]
        public IActionResult Index()
        {
            return View(_authorRepository.Authors);
        }

        [Route("[action]")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Author author)
        {
            if (ModelState.IsValid)
            {
                _authorRepository.SaveAuthor(author);
                TempData["message"] = $"{author.Name} {author.Surname} has been saved";
                return RedirectToAction(nameof(Index));
            }

            return View(author);
        }
    }
}