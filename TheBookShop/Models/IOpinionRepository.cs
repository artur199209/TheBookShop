using System.Linq;

namespace TheBookShop.Models
{
    public interface IOpinionRepository
    {
        IQueryable<Opinion> Opinions { get; }
        void SaveOpinion(Opinion opinion);
    }
}