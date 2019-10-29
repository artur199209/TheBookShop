using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public class EFAuthorRepository : IAuthorRepository
    {
        private ApplicationDbContext _applicationDbContext;

        public EFAuthorRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IQueryable<Author> Authors => _applicationDbContext.Authors.Include(x => x.Products);

        public void SaveAuthor(Author author)
        {
            try
            {
                if (author.AuthorId == 0)
                {
                    _applicationDbContext.Authors.Add(author);
                }
                else
                {
                    var authorEntry = _applicationDbContext.Authors.FirstOrDefault(x => x.AuthorId == author.AuthorId);

                    if (authorEntry != null)
                    {
                        authorEntry.Name = author.Name;
                        authorEntry.Surname = author.Surname;
                        authorEntry.Notes = author.Notes;
                    }
                }

                _applicationDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Author DeleteAuthor(int authorId)
        {
            var author = _applicationDbContext.Authors.FirstOrDefault(x => x.AuthorId == authorId);

            if (author != null)
            {
                _applicationDbContext.Authors.Remove(author);
                _applicationDbContext.SaveChanges();
            }

            return author;
        }

        public Author GetAuthorById(int authorId)
        {
            return Authors.FirstOrDefault(x => x.AuthorId == authorId);
        }
    }
}