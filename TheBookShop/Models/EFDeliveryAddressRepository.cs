using System;
using System.Linq;

namespace TheBookShop.Models
{
    public class EFDeliveryAddressRepository : IDeliveryAdressRepository
    {
        private ApplicationDbContext applicationDbContext;

        public EFDeliveryAddressRepository(ApplicationDbContext _applicationDbContext)
        {
            applicationDbContext = _applicationDbContext;
        }

        public IQueryable<DeliveryAddress> DeliveryAddresses => applicationDbContext.DeliveryAddresses;

        public void SaveDeliveryAddress(DeliveryAddress deliveryAddress)
        {
            try
            {
                applicationDbContext.DeliveryAddresses.Add(deliveryAddress);
                applicationDbContext.SaveChanges();
            }
            catch(Exception ex)
            {

            }
        }
    }
}