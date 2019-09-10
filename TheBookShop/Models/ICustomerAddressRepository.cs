using System.Linq;

namespace TheBookShop.Models
{
    public interface ICustomerAddressRepository
    {
        IQueryable<CustomerAddress> DeliveryAddresses { get; }
        void SaveDeliveryAddress(CustomerAddress deliveryAddress);
    }
}