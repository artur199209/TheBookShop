using System.Linq;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public interface IOpinionRepository
    {
        IQueryable<Opinion> Opinions { get; }
        void SaveOpinion(Opinion opinion);
    }
}