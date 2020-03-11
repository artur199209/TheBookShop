using System;
using System.Linq;
using Serilog;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public class EFDeliveryAddressRepository : IDeliveryAdressRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EFDeliveryAddressRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IQueryable<DeliveryAddress> DeliveryAddresses => _applicationDbContext.DeliveryAddresses;

        public void SaveDeliveryAddress(DeliveryAddress deliveryAddress)
        {
            try
            {
                Log.Information("Adding new delivery address...");
                _applicationDbContext.DeliveryAddresses.Add(deliveryAddress);
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