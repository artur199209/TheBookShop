using System;
using System.Linq;
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
                _applicationDbContext.Attach(opinion.Product);
                _applicationDbContext.Opinions.Add(opinion);
                _applicationDbContext.SaveChanges();
            }
            catch(Exception ex)
            {

            }
        }
    }
}