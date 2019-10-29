using System;
using System.Linq;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public class EFCustomerAddressRepository : ICustomerAddressRepository
    {
        private ApplicationDbContext applicationDbContext;

        public EFCustomerAddressRepository(ApplicationDbContext _applicationDbContext)
        {
            applicationDbContext = _applicationDbContext;
        }

        public IQueryable<CustomerAddress> DeliveryAddresses => applicationDbContext.CustomerAddresses;

        public void SaveDeliveryAddress(CustomerAddress customerAddress)
        {
            try
            {
                if (customerAddress.CustomerAddressId == 0)
                {
                    applicationDbContext.CustomerAddresses.Add(customerAddress);
                }
                else
                {
                    var customerAddressEntry = applicationDbContext.CustomerAddresses
                        .FirstOrDefault(x => x.CustomerAddressId == customerAddress.CustomerAddressId);

                    if (customerAddressEntry != null)
                    {
                        customerAddressEntry.Country = customerAddress.Country;
                        customerAddressEntry.City = customerAddress.City;
                        customerAddressEntry.HomeNumber = customerAddress.HomeNumber;
                        customerAddressEntry.ZipCode = customerAddress.ZipCode;
                    }
                }

                applicationDbContext.SaveChanges();
            }
            catch(Exception ex)
            {

            }
        }
    }
}