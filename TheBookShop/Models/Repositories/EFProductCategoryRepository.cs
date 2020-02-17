using System.Linq;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public class EFProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EFProductCategoryRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IQueryable<ProductCategory> ProductCategories => _applicationDbContext.ProductCategories;

        public void SaveCategory(ProductCategory productCategory)
        {
            if (productCategory.ProductCategoryId == 0)
            {
                _applicationDbContext.ProductCategories.Add(productCategory);
            }
            else
            {
                var category = _applicationDbContext.ProductCategories.FirstOrDefault(x =>
                    x.ProductCategoryId == productCategory.ProductCategoryId);

                if (category != null)
                {
                    category.Name = productCategory.Name;
                }
            }

            _applicationDbContext.SaveChanges();
        }
    }
}