using System.Collections.Generic;
using TheBookShop.Models;
using TheBookShop.Models.ViewModel;

namespace TheBookShop.Infrastructure
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}