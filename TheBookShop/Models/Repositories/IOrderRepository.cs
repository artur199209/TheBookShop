using System.Linq;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        void SaveOrder(Order order);
    }
}