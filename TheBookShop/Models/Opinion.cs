using System;

namespace TheBookShop.Models
{
    public class Opinion
    {
        public int OpinionId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public string OpinionDescription { get; set; }
        public DateTime OpinionDate { get; set; }
    }
}