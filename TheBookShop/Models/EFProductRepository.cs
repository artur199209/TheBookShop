using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBookShop.Models
{
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext context;

        public EFProductRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Product> Products => context.Products;
        public void SaveProduct(Product product)
        {
            if (product.ProductId == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product productEntry = context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);

                if (productEntry != null)
                {
                    productEntry.Title = product.Title;
                    productEntry.Category = product.Category;
                    productEntry.Description = product.Description;
                    productEntry.Price = product.Price;
                }
            }

            context.SaveChanges();
        }

        public Product DeleteProduct(int productId)
        {
            Product productEntry = context.Products.FirstOrDefault(p => p.ProductId == productId);

            if (productEntry != null)
            {
                context.Products.Remove(productEntry);
                context.SaveChanges();
            }

            return productEntry;
        }
    }
}
