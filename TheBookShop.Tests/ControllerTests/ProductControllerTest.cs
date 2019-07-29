﻿using System.Linq;
using Moq;
using TheBookShop.Controllers;
using TheBookShop.Infrastructure;
using TheBookShop.Models;
using TheBookShop.Models.ViewModel;
using Xunit;

namespace TheBookShop.Tests
{
    public class ProductControllerTest
    {
        [Fact]
        public void Can_Paginate()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "Product1"},
                new Product {ProductId = 2, Name = "Product2"},
                new Product {ProductId = 3, Name = "Product3"},
                new Product {ProductId = 4, Name = "Product4"},
                new Product {ProductId = 5, Name = "Product5"}
            }).AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            var products = result?.Products.ToArray();
            
            Assert.True(products?.Length == 2);
            Assert.Equal("Product4", products[0].Name);
            Assert.Equal("Product5", products[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "Product1"},
                new Product {ProductId = 2, Name = "Product2"},
                new Product {ProductId = 3, Name = "Product3"},
                new Product {ProductId = 4, Name = "Product4"},
                new Product {ProductId = 5, Name = "Product5"}
            }).AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            PagingInfo pageInfo = result.PagingInfo;

            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
            Assert.Equal(2, pageInfo.CurrentPage);
        }
    }
}