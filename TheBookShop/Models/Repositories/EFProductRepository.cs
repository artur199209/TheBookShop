using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext context;

        public EFProductRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Product> Products => context.Products
            .Include(o => o.Opinions)
            .Include(a => a.Author)
            .Include(c => c.Category);

        public void SaveProduct(Product product)
        {
            try
            {
                if (product.ProductId == 0)
                {
                    Log.Information("Adding new product...");
                    context.Products.Add(product);
                }
                else
                {
                    Log.Information($"Updating existing product {product.ProductId}...");

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
                        productEntry.SalesType = product.SalesType;
                        productEntry.IsProductInPromotion = product.IsProductInPromotion;
                        productEntry.PromotionalPrice = product.PromotionalPrice;
                    }
                }

                context.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Error($"Error while saving order...");
                Log.Error(e.Message);
                Log.Error(e.StackTrace);
                Console.WriteLine(e);
            }
        }

        public Product DeleteProduct(int productId)
        {
            Product productEntry = null;

            try
            {
                productEntry = context.Products.FirstOrDefault(p => p.ProductId == productId);

                if (productEntry != null)
                {
                    Log.Information($"Deleting product {productEntry.ProductId}...");
                    context.Products.Remove(productEntry);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error while saving order...");
                Log.Error(e.Message);
                Log.Error(e.StackTrace);
                Console.WriteLine(e);
            }

            return productEntry;
        }
        
    }
}
