using Microsoft.AspNetCore.Mvc;
using TheBookShop.Models;

namespace TheBookShop.Controllers
{
    public class AuthorController : Controller
    {
        private IAuthorRepository _authorRepository;

        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        
        public IActionResult Index()
        {
            return View(_authorRepository.Authors);
        }

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