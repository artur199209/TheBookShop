using System.Linq;

namespace TheBookShop.Models
{
    interface IProductRepository
    {
        IQueryable<Product> Products { get; }
    }
}