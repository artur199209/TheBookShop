using System;
using System.Linq;
using Serilog;
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
            try
            {
                if (productCategory.ProductCategoryId == 0)
                {
                    Log.Information($"Adding product category {productCategory.Name}");
                    _applicationDbContext.ProductCategories.Add(productCategory);
                }
                else
                {
                    Log.Information($"Updating product category {productCategory.Name}");

                    var category = _applicationDbContext.ProductCategories.FirstOrDefault(x =>
                        x.ProductCategoryId == productCategory.ProductCategoryId);

                    if (category != null)
                    {
                        category.Name = productCategory.Name;
                    }
                }

                _applicationDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Error($"Error while saving order...");
                Log.Error(e.Message);
                Log.Error(e.StackTrace);
                Console.WriteLine(e);
            }
        }
    }
}