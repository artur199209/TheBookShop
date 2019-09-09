using System;
using System.Linq;

namespace TheBookShop.Models
{
    public class EFAuthorRepository : IAuthorRepository
    {
        private ApplicationDbContext _applicationDbContext;

        public EFAuthorRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IQueryable<Author> Authors => _applicationDbContext.Authors;

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
    }
}