using System.Collections.Generic;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Areas.Admin.Model
{
    public class DeliveryPaymentModificationModel
    {
        public DeliveryMethod DeliveryMethod { get; set; }
        public List<int> IdsToAdd { get; set; }
        public List<int> IdsToDelete { get; set; }
    }
}