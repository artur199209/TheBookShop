using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public class EFAuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

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
                    Log.Information("Adding new author...");
                    _applicationDbContext.Authors.Add(author);
                }
                else
                {
                    Log.Information($"Updating author wit Id: {author.AuthorId}...");

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
                Log.Error($"Error while adding/updating author...");
                Log.Error(e.Message);
                Log.Error(e.StackTrace);
                Console.WriteLine(e);
            }
        }

        public Author DeleteAuthor(int authorId)
        {
            Author author = null;
            try
            {
                author = _applicationDbContext.Authors.FirstOrDefault(x => x.AuthorId == authorId);

                if (author != null)
                {
                    _applicationDbContext.Authors.Remove(author);
                    _applicationDbContext.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error while deleting author...");
                Log.Error(e.Message);
                Log.Error(e.StackTrace);
                Console.WriteLine(e);
            }
            
            return author;
        }

        public Author GetAuthorById(int authorId)
        {
            return Authors.FirstOrDefault(x => x.AuthorId == authorId);
        }
    }
}