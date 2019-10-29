using System.Collections.Generic;
using TheBookShop.Models.DataModels;
using TheBookShop.Models.ViewModels;

namespace TheBookShop.Areas.Admin.Model
{
    public class AuthorListViewModel
    {
        public IEnumerable<Author> Authors { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}