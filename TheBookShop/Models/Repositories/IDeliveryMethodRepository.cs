using System.Linq;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public interface IDeliveryMethodRepository
    {
        IQueryable<DeliveryMethod> DeliveryMethods { get; }
        void SaveDeliveryMethod(DeliveryMethod deliveryMethod);
    }
}