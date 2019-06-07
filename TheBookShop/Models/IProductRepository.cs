using System.Linq;

namespace TheBookShop.Models
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
    }
}