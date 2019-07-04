﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using TheBookShop.Controllers;
using TheBookShop.Models;
using Xunit;

namespace TheBookShop.Tests.ControllerTests
{
    public class AdminControllerTests
    {
        [Fact]
        public void Index_Contains_All_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "Product1"},
                new Product {ProductId = 2, Name = "Product2"},
                new Product {ProductId = 3, Name = "Product3"},
            }).AsQueryable());

            AdminController controller = new AdminController(mock.Object);
            Product[] results = GetViewModel<IEnumerable<Product>>(controller.Index())?.ToArray();

            Assert.Equal(3, results?.Length);
            Assert.Equal("Product1", results?[0].Name);
            Assert.Equal("Product2", results?[1].Name);
            Assert.Equal("Product3", results?[2].Name);
        }

        [Fact]
        public void Can_Edit_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "Product1"},
                new Product {ProductId = 2, Name = "Product2"},
                new Product {ProductId = 3, Name = "Product3"},
            }).AsQueryable());

            AdminController controller = new AdminController(mock.Object);
            Product p1 = GetViewModel<Product>(controller.Edit(1));
            Product p2 = GetViewModel<Product>(controller.Edit(2));
            Product p3 = GetViewModel<Product>(controller.Edit(3));

            Assert.Equal(1, p1.ProductId);
            Assert.Equal(2, p2.ProductId);
            Assert.Equal(3, p3.ProductId);
        }

        [Fact]
        public void Cannot_Edit_Non_Existing_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "Product1"},
                new Product {ProductId = 2, Name = "Product2"},
                new Product {ProductId = 3, Name = "Product3"},
            }).AsQueryable());

            AdminController controller = new AdminController(mock.Object);
            Product p1 = GetViewModel<Product>(controller.Edit(4));

            Assert.Null(p1);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            AdminController controller = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };

            Product product = new Product() {Name = "Test"};
            IActionResult result = controller.Edit(product);
            mock.Verify(m => m.SaveProduct(product));

            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult)?.ActionName);

        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            AdminController controller = new AdminController(mock.Object);
            controller.ModelState.AddModelError("error", "error");
            Product product = new Product() { Name = "Test" };

            IActionResult result = controller.Edit(product);

            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Product()
        {
            Product prod4 = new Product(){ProductId = 4, Name = "Product4"};

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductId = 1, Name = "Product1"},
                new Product {ProductId = 2, Name = "Product2"},
                new Product {ProductId = 3, Name = "Product3"},
            }).AsQueryable());

            AdminController controller = new AdminController(mock.Object);
            controller.Delete(prod4.ProductId);

            mock.Verify(m => m.DeleteProduct(prod4.ProductId));
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}