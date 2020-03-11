using System;
using System.Linq;
using Serilog;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public class EFCustomerAddressRepository : ICustomerAddressRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EFCustomerAddressRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IQueryable<CustomerAddress> DeliveryAddresses => _applicationDbContext.CustomerAddresses;

        public void SaveDeliveryAddress(CustomerAddress customerAddress)
        {
            try
            {
                if (customerAddress.CustomerAddressId == 0)
                {
                    Log.Information("Adding new customer address...");
                    _applicationDbContext.CustomerAddresses.Add(customerAddress);
                }
                else
                {
                    Log.Information("Updating existing customer address...");
                    var customerAddressEntry = _applicationDbContext.CustomerAddresses
                        .FirstOrDefault(x => x.CustomerAddressId == customerAddress.CustomerAddressId);

                    if (customerAddressEntry != null)
                    {
                        customerAddressEntry.Country = customerAddress.Country;
                        customerAddressEntry.City = customerAddress.City;
                        customerAddressEntry.HomeNumber = customerAddress.HomeNumber;
                        customerAddressEntry.ZipCode = customerAddress.ZipCode;
                    }
                }

                _applicationDbContext.SaveChanges();
            }
            catch(Exception e)
            {
                Log.Error($"Error while adding/updating author...");
                Log.Error(e.Message);
                Log.Error(e.StackTrace);
                Console.WriteLine(e);
            }
        }
    }
}