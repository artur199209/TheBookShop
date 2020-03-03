using System.Collections.Generic;
using System.Linq;

namespace TheBookShop.Models.DataModels
{
    public class Cart
    {
        private readonly List<CartLine> _lineCollection = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = _lineCollection.FirstOrDefault(p => p.Product.ProductId == product.ProductId);

            if (line == null)
            {
                _lineCollection.Add(new CartLine()
                {
                    Product =  product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void DecreaseProductCount(Product product)
        {
            CartLine line = _lineCollection.FirstOrDefault(p => p.Product.ProductId == product.ProductId);

            if (line != null)
            {
                line.Quantity -= 1;

                if (line.Quantity == 0)
                {
                    RemoveLine(product);
                }
            }
        }

        public virtual void RemoveLine(Product product)
        {
            _lineCollection.RemoveAll(l => l.Product.ProductId == product.ProductId);
        }

        public virtual decimal ComputeTotalValue() => _lineCollection.Sum(x => x.LineCost());

        public virtual void Clear() => _lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => _lineCollection;
    }

    public class CartLine
    {
        public int CartLineId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public decimal LineCost()
        {
            return Quantity * (Product.IsProductInPromotion ? Product.PromotionalPrice : Product.Price);
        }
    }
}