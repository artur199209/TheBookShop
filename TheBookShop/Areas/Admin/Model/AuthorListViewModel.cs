using System.Collections.Generic;
using TheBookShop.Models;
using TheBookShop.Models.ViewModel;

namespace TheBookShop.Areas.Admin.Model
{
    public class AuthorListViewModel
    {
        public IEnumerable<Author> Authors { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}