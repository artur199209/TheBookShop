using System;

namespace TheBookShop.Models.DataModels
{
    public class Opinion
    {
        public int OpinionId { get; set; }
        public Product Product { get; set; }
        public Customer Customer { get; set; }
        public string OpinionDescription { get; set; }
        public DateTime OpinionDate { get; set; }
    }
}