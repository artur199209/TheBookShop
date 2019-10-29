using System;
using System.Linq;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public class EFOpinionRepository : IOpinionRepository
    {
        private ApplicationDbContext applicationDbContext;

        public EFOpinionRepository(ApplicationDbContext _applicationDbContext)
        {
            applicationDbContext = _applicationDbContext;
        }

        public IQueryable<Opinion> Opinions => applicationDbContext.Opinions;

        public void SaveOpinion(Opinion opinion)
        {
            try
            {
                applicationDbContext.Opinions.Add(opinion);
                applicationDbContext.SaveChanges();
            }
            catch(Exception ex)
            {

            }
        }
    }
}