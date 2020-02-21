using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TheBookShop.Areas.Admin.Controllers;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.Repositories;
using Xunit;

namespace TheBookShop.Tests.AdminTests.ControllerTests
{
    public class ProductCategoryControllerTests
    {
        private readonly Mock<IProductCategoryRepository> _productCategoryRepository;
        private readonly ProductCategory _productCategory;

        public ProductCategoryControllerTests()
        {
            _productCategoryRepository = new Mock<IProductCategoryRepository>();
            _productCategoryRepository.Setup(x => x.ProductCategories).Returns(new[]
            {
                new ProductCategory { ProductCategoryId = 1 ,Name = "Category1" },
                new ProductCategory { ProductCategoryId = 2 ,Name = "Category2" },
                new ProductCategory { ProductCategoryId = 3 ,Name = "Category3" }
            }.AsQueryable());

            _productCategory = new ProductCategory()
            {
                ProductCategoryId = 100,
                Name = "Test"
            };
        }

        [Fact]
        public void Index_Contains_All_Product_Categories()
        {
            var productCategoryController = new ProductCategoryController(_productCategoryRepository.Object);
            var result = GetViewModel<IEnumerable<ProductCategory>>(productCategoryController.Index());

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void Can_Edit_Product_Category()
        {
            var controller = new ProductCategoryController(_productCategoryRepository.Object);
            ProductCategory category1 = GetViewModel<ProductCategory>(controller.Edit(1));
            ProductCategory category2 = GetViewModel<ProductCategory>(controller.Edit(2));
            ProductCategory category3 = GetViewModel<ProductCategory>(controller.Edit(3));

            Assert.Equal(1, category1.ProductCategoryId);
            Assert.Equal(2, category2.ProductCategoryId);
            Assert.Equal(3, category3.ProductCategoryId);
        }

        [Fact]
        public void Cannot_Edit_Non_Existing_Product_Category()
        {
            var controller = new ProductCategoryController(_productCategoryRepository.Object);
            var category = GetViewModel<ProductCategory>(controller.Edit(4));

            Assert.Null(category);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();

            var controller = new ProductCategoryController(_productCategoryRepository.Object)
            {
                TempData = tempData.Object
            };

            IActionResult result = controller.Edit(_productCategory);
            _productCategoryRepository.Verify(m => m.SaveCategory(_productCategory));

            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult)?.ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            var controller = new ProductCategoryController(_productCategoryRepository.Object);
            controller.ModelState.AddModelError("error", "error");

            IActionResult result = controller.Edit(_productCategory);

            _productCategoryRepository.Verify(m => m.SaveCategory(It.IsAny<ProductCategory>()), Times.Never);
            Assert.IsType<ViewResult>(result);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }
    }
}