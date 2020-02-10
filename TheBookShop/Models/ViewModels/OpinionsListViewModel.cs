using System.Collections.Generic;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.ViewModels
{
    public class OpinionsListViewModel
    {
        public IEnumerable<Opinion> Opinions { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public int ProductId { get; set; }
    }
}