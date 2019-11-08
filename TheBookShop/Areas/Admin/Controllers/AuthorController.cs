using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TheBookShop.Areas.Admin.Model;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using TheBookShop.Models.ViewModels;

namespace TheBookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        public int PageSize = 4;

        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        
        [Route("")]
        [Route("[action]")]
        [Route("[action]/{page}")]
        public IActionResult Index(int page = 1)
        {
            return View(new AuthorListViewModel
            {
                Authors = _authorRepository.Authors.Skip((page - 1) * PageSize).Take(PageSize).ToList(),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _authorRepository.Authors.Count()
                }
            });
        }

        [Route("[action]")]
        public IActionResult Create()
        {
            return View(nameof(Edit), new Author());
        }

        [Route("[action]")]
        public IActionResult Edit(int authorId)
        {
            var author = _authorRepository.Authors.FirstOrDefault(x => x.AuthorId == authorId);
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public IActionResult Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                _authorRepository.SaveAuthor(author);
                TempData["message"] = $"{author.Name} {author.Surname} has been saved";
                return RedirectToAction(nameof(Index));
            }

            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public IActionResult Delete(int authorId)
        {
            var deletedAuthor = _authorRepository.DeleteAuthor(authorId);

            if (deletedAuthor != null)
            {
                TempData["message"] = $"{deletedAuthor.Name} {deletedAuthor.Surname } was deleted";
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("[action]")]
        public IActionResult Books(int authorId)
        {
            var authorWithBooks = _authorRepository.GetAuthorById(authorId);
            return View(authorWithBooks);
        }
    }
}