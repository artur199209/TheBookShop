using System.Collections.Generic;
using TheBookShop.Models;
using TheBookShop.Models.ViewModel;

namespace TheBookShop.Areas.Admin.Model
{
    public class AccountListViewModel
    {
        public IEnumerable<AppUser> Accounts { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}