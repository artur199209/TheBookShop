using System.Collections.Generic;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.ViewModels;

namespace TheBookShop.Areas.Admin.Model
{
    public class AccountListViewModel
    {
        public IEnumerable<AppUser> Accounts { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}