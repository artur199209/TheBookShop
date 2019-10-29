using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using TheBookShop.Infrastructure;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using Xunit;

namespace TheBookShop.Tests.InfrastructureTests
{
    public class SelectOptionTagHelperTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly TagHelperOutput _output;
        private readonly TagHelperContext _ctx;

        public SelectOptionTagHelperTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();

            var mockContent = new Mock<TagHelperContent>();

            _ctx = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(), "");

            _output = new TagHelperOutput("select",
                new TagHelperAttributeList(),
                (cache, encoder) => Task.FromResult(mockContent.Object));
        }

        [Fact]
        public async Task Output_Is_Null_When_There_Are_No_Authors()
        {
            _authorRepositoryMock.Setup(x => x.Authors).Returns(new List<Author>().AsQueryable);

            var selectOptionTagHelper = new SelectOptionTagHelper(_authorRepositoryMock.Object);
            await selectOptionTagHelper.ProcessAsync(_ctx, _output);

            Assert.Equal("<option disabled selected>Wybierz autora</option>", _output.Content.GetContent());
        }

        [Fact]
        public async Task Output_Contains_Authors()
        {
            _authorRepositoryMock.Setup(x => x.Authors).Returns(new []
            {
                new Author { Name = "Name1", Surname = "Surname1" },
                new Author { Name = "Name2", Surname = "Surname2" }
            }.AsQueryable());

            var selectOptionTagHelper = new SelectOptionTagHelper(_authorRepositoryMock.Object);
            await selectOptionTagHelper.ProcessAsync(_ctx, _output);

            Assert.Equal("<option disabled selected>Wybierz autora</option><option value=0>Name1 Surname1 </option><option value=0>Name2 Surname2 </option>", _output.Content.GetContent());
        }
    }
}