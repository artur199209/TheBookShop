using System.Linq;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public interface IAuthorRepository
    {
        IQueryable<Author> Authors { get; }
        void SaveAuthor(Author author);
        Author DeleteAuthor(int authorId);
        Author GetAuthorById(int authorId);
    }
}