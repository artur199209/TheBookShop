using System.Linq;

namespace TheBookShop.Models
{
    public interface IDeliveryAdressRepository
    {
        IQueryable<DeliveryAddress> DeliveryAddresses { get; }
        void SaveDeliveryAddress(DeliveryAddress deliveryAddress);
    }
}