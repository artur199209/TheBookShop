using System.Linq;
using TheBookShop.Models;
using Xunit;

namespace TheBookShop.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_Items_To_Cart()
        {
            Product p1 = new Product {ProductId = 1, Name = "Product1"};
            Product p2 = new Product {ProductId = 2, Name = "Product2"};

            Cart cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);

            var results = cart.Lines.ToArray();

            Assert.Equal(2, results.Length);
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(p2, results[1].Product);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            Product p1 = new Product { ProductId = 1, Name = "Product1" };
            Product p2 = new Product { ProductId = 2, Name = "Product2" };

            Cart cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p2, 10);
            cart.AddItem(p1, 1);

            var results = cart.Lines.OrderBy(c => c.Product.ProductId).ToArray();

            Assert.Equal(2, results.Length);
            Assert.Equal(2, results[0].Quantity);
            Assert.Equal(11, results[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Item_From_Cart()
        {
            Product p1 = new Product { ProductId = 1, Name = "Product1" };
            Product p2 = new Product { ProductId = 2, Name = "Product2" };
            Product p3 = new Product { ProductId = 3, Name = "Product3" };
            Product p4 = new Product { ProductId = 4, Name = "Product4" };
            Product p5 = new Product { ProductId = 5, Name = "Product5" };

            Cart cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p3, 1);
            cart.AddItem(p4, 1);
            cart.AddItem(p5, 1);

            cart.RemoveLine(p1);
            cart.RemoveLine(p4);
            var results = cart.Lines.ToArray();
            
            Assert.Equal(3, results.Length);
            Assert.Equal(p2, results[0].Product);
            Assert.Equal(p3, results[1].Product);
            Assert.Equal(p5, results[2].Product);
        }

        [Fact]
        public void Can_Clear_Lines()
        {
            Product p1 = new Product { ProductId = 1, Name = "Product1" };
            Product p2 = new Product { ProductId = 2, Name = "Product2" };
            Product p3 = new Product { ProductId = 3, Name = "Product3" };
            Product p4 = new Product { ProductId = 4, Name = "Product4" };

            Cart cart = new Cart();
            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p3, 1);
            cart.AddItem(p4, 1);

            cart.Clear();
            
            Assert.Equal(0, cart.Lines.Count());
        }

        [Fact]
        public void Calculate_Total_Value()
        {
            Product p1 = new Product { ProductId = 1, Name = "Product1", Price = 5};
            Product p2 = new Product { ProductId = 2, Name = "Product2", Price = 10};
            Product p3 = new Product { ProductId = 3, Name = "Product3", Price = 11};
            Product p4 = new Product { ProductId = 4, Name = "Product4", Price = 3};

            Cart cart = new Cart();
            cart.AddItem(p1, 10);
            cart.AddItem(p2, 5);
            cart.AddItem(p3, 9);
            cart.AddItem(p4, 10);

            var result = cart.ComputeTotalValue();

            Assert.Equal(229, result);
        }
    }
}