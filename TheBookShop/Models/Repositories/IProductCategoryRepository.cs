using System.Linq;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public interface IProductCategoryRepository
    {
        IQueryable<ProductCategory> ProductCategories { get; }
        void SaveCategory(ProductCategory productCategory);
    }
}