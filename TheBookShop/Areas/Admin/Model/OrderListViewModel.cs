using System.Collections.Generic;
using TheBookShop.Models;
using TheBookShop.Models.ViewModel;

namespace TheBookShop.Areas.Admin.Model
{
    public class OrderListViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}