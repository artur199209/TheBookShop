using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TheBookShop.Models.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext context;

        public EFProductRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Product> Products => context.Products
            .Include(o => o.Opinions)
            .Include(a => a.Author);

        public void SaveProduct(Product product)
        {
            try
            {
                if (product.ProductId == 0)
                {
                    context.Products.Add(product);
                }
                else
                {
                    var productEntry = context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);

                    if (productEntry != null)
                    {
                        productEntry.Title = product.Title;
                        productEntry.Category = product.Category;
                        productEntry.Description = product.Description;
                        productEntry.Price = product.Price;
                        productEntry.Author = product.Author;
                        productEntry.Cover = product.Cover;
                        productEntry.Image = product.Image;
                        productEntry.NumberOfPages = product.NumberOfPages;
                        productEntry.PublishingHouse = product.PublishingHouse;
                        productEntry.QuantityInStock = product.QuantityInStock;
                        productEntry.Subcategory = product.Subcategory;
                    }
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
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
