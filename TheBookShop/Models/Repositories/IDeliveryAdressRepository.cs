using System.Linq;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public interface IDeliveryAdressRepository
    {
        IQueryable<DeliveryAddress> DeliveryAddresses { get; }
        void SaveDeliveryAddress(DeliveryAddress deliveryAddress);
    }
}