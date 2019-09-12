using System.Linq;

namespace TheBookShop.Models
{
    public interface IAuthorRepository
    {
        IQueryable<Author> Authors { get; }
        void SaveAuthor(Author author);
        Author DeleteAuthor(int authorId);
        Author GetAuthorById(int authorId);
    }
}