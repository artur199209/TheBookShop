using System.Linq;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public interface ICustomerAddressRepository
    {
        IQueryable<CustomerAddress> DeliveryAddresses { get; }
        void SaveDeliveryAddress(CustomerAddress deliveryAddress);
    }
}