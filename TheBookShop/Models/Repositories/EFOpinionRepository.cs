using System;
using System.Linq;
using Serilog;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public class EFOpinionRepository : IOpinionRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EFOpinionRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IQueryable<Opinion> Opinions => _applicationDbContext.Opinions;

        public void SaveOpinion(Opinion opinion)
        {
            try
            {
                Log.Information($"Adding new opinion for product: {opinion.Product.ProductId}");
                _applicationDbContext.Attach(opinion.Product);
                _applicationDbContext.Opinions.Add(opinion);
                _applicationDbContext.SaveChanges();
            }
            catch(Exception e)
            {
                Log.Error($"Error while adding/updating author...");
                Log.Error(e.Message);
                Log.Error(e.StackTrace);
                Console.WriteLine(e);
            }
        }
    }
}