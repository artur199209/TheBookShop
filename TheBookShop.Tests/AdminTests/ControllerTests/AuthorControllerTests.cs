using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Models;
using Xunit;

namespace TheBookShop.Tests.ControllerTests
{
    public class AuthorControllerTests
    {
        [Fact]
        public void Index_Contains_All_Authors()
        {
            Mock<IAuthorRepository> authorRepoMock = new Mock<IAuthorRepository>();
            authorRepoMock.Setup(m => m.Authors).Returns((new []
            {
                new Author {AuthorId = 1, Name = "Jan", Surname = "Kowalski"},
                new Author {AuthorId = 2, Name = "Marcin", Surname = "Nowak"},
                new Author {AuthorId = 3, Name = "Jacek", Surname = "Nowak"},
            }).AsQueryable());

            AuthorController controller = new AuthorController(authorRepoMock.Object);
            Author[] results = GetViewModel<IEnumerable<Author>>(controller.Index())?.ToArray();

            Assert.Equal(3, results?.Length);
            Assert.Equal("Jan", results?[0].Name);
            Assert.Equal("Marcin", results?[1].Name);
            Assert.Equal("Jacek", results?[2].Name);
        }

        [Fact]
        public void Can_Edit_Author()
        {
            Mock<IAuthorRepository> authorRepoMock = new Mock<IAuthorRepository>();
            authorRepoMock.Setup(m => m.Authors).Returns((new []
            {
                new Author {AuthorId = 1, Name = "Jan", Surname = "Kowalski"},
                new Author {AuthorId = 2, Name = "Marcin", Surname = "Nowak"},
                new Author {AuthorId = 3, Name = "Jacek", Surname = "Nowak"},
            }).AsQueryable());

            AuthorController controller = new AuthorController(authorRepoMock.Object);
            Author p1 = GetViewModel<Author>(controller.Edit(1));
            Author p2 = GetViewModel<Author>(controller.Edit(2));
            Author p3 = GetViewModel<Author>(controller.Edit(3));

            Assert.Equal(1, p1.AuthorId);
            Assert.Equal(2, p2.AuthorId);
            Assert.Equal(3, p3.AuthorId);
        }

        [Fact]
        public void Cannot_Edit_Non_Existing_Author()
        {
            Mock<IAuthorRepository> authorRepoMock = new Mock<IAuthorRepository>();
            authorRepoMock.Setup(m => m.Authors).Returns((new []
            {
                new Author {AuthorId = 1, Name = "Jan", Surname = "Kowalski"},
                new Author {AuthorId = 2, Name = "Marcin", Surname = "Nowak"},
                new Author {AuthorId = 3, Name = "Jacek", Surname = "Nowak"},
            }).AsQueryable());

            AuthorController controller = new AuthorController(authorRepoMock.Object);
            Author author = GetViewModel<Author>(controller.Edit(4));

            Assert.Null(author);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            Mock<IAuthorRepository> authorRepoMock = new Mock<IAuthorRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            AuthorController controller = new AuthorController(authorRepoMock.Object)
            {
                TempData = tempData.Object
            };

            Author author = new Author() { Name = "Jan" };
            IActionResult result = controller.Edit(author);
            authorRepoMock.Verify(m => m.SaveAuthor(author));

            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult)?.ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            Mock<IAuthorRepository> authorRepoMock = new Mock<IAuthorRepository>();

            AuthorController controller = new AuthorController(authorRepoMock.Object);
            controller.ModelState.AddModelError("error", "error");
            Author author = new Author() { Name = "Jan" };

            IActionResult result = controller.Edit(author);

            authorRepoMock.Verify(m => m.SaveAuthor(It.IsAny<Author>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Author()
        {
            Author author = new Author() { AuthorId = 4, Name = "Jan" };
            
            Mock<IAuthorRepository> authorRepoMock = new Mock<IAuthorRepository>();
            authorRepoMock.Setup(m => m.Authors).Returns((new []
            {
                new Author {AuthorId = 1, Name = "Jan", Surname = "Kowalski"},
                new Author {AuthorId = 2, Name = "Marcin", Surname = "Nowak"},
                new Author {AuthorId = 3, Name = "Jacek", Surname = "Nowak"},
            }).AsQueryable());

            AuthorController controller = new AuthorController(authorRepoMock.Object);
            controller.Delete(author.AuthorId);

            authorRepoMock.Verify(m => m.DeleteAuthor(author.AuthorId));
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}