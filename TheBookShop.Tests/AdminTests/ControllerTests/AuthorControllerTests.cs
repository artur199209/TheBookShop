using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Areas.Admin.Model;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using TheBookShop.Tests.Helper;
using Xunit;

namespace TheBookShop.Tests.AdminTests.ControllerTests
{
    public class AuthorControllerTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Author _author;

        public AuthorControllerTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _authorRepositoryMock.Setup(m => m.Authors).Returns(new[]
            {
                new Author { AuthorId = 1, Name = "Author1", Surname = "Surname1" },
                new Author { AuthorId = 2, Name = "Author2", Surname = "Surname2" },
                new Author { AuthorId = 3, Name = "Author3", Surname = "Surname3" }
            }.AsQueryable());

            _author = new Author { AuthorId = 4, Name = "Author4" };
        }

        [Fact]
        public void Index_Contains_All_Authors()
        {
            AuthorController controller = new AuthorController(_authorRepositoryMock.Object);
            var results = CastHelper.GetViewModel<AuthorListViewModel>(controller.Index()).Authors.ToArray();
            
            Assert.Equal(3, results.Length);
            Assert.Equal("Author1", results[0].Name);
            Assert.Equal("Author2", results[1].Name);
            Assert.Equal("Author3", results[2].Name);
        }

        [Fact]
        public void Can_Edit_Author()
        {
            AuthorController controller = new AuthorController(_authorRepositoryMock.Object);
            Author p1 = CastHelper.GetViewModel<Author>(controller.Edit(1));
            Author p2 = CastHelper.GetViewModel<Author>(controller.Edit(2));
            Author p3 = CastHelper.GetViewModel<Author>(controller.Edit(3));

            Assert.Equal(1, p1.AuthorId);
            Assert.Equal(2, p2.AuthorId);
            Assert.Equal(3, p3.AuthorId);
        }

        [Fact]
        public void Cannot_Edit_Non_Existing_Author()
        {
            AuthorController controller = new AuthorController(_authorRepositoryMock.Object);
            Author author = CastHelper.GetViewModel<Author>(controller.Edit(4));

            Assert.Null(author);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            AuthorController controller = new AuthorController(_authorRepositoryMock.Object)
            {
                TempData = tempData.Object
            };
            
            IActionResult result = controller.Edit(_author);
            _authorRepositoryMock.Verify(m => m.SaveAuthor(_author));

            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult)?.ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            AuthorController controller = new AuthorController(_authorRepositoryMock.Object);
            controller.ModelState.AddModelError("error", "error");

            IActionResult result = controller.Edit(_author);

            _authorRepositoryMock.Verify(m => m.SaveAuthor(It.IsAny<Author>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Author()
        {
            AuthorController controller = new AuthorController(_authorRepositoryMock.Object);
            controller.Delete(_author.AuthorId);

            _authorRepositoryMock.Verify(m => m.DeleteAuthor(_author.AuthorId));
        }

        [Fact]
        public void Can_Send_Pagination()
        {
            AuthorController controller = new AuthorController(_authorRepositoryMock.Object);
            var result = CastHelper.GetViewModel<AuthorListViewModel>(controller.Index()).PagingInfo;

            Assert.Equal(3, result.TotalItems);
            Assert.Equal(1, result.TotalPages);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(4, result.ItemsPerPage);
        }
    }
}