using System.Collections.Generic;
using TheBookShop.Models;
using TheBookShop.Models.ViewModel;

namespace TheBookShop.Areas.Admin.Model
{
    public class PaymentListViewModel
    {
        public IEnumerable<Payment> Payments { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}