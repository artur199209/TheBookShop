using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public class EFDeliveryMethodRepository : IDeliveryMethodRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EFDeliveryMethodRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IQueryable<DeliveryMethod> DeliveryMethods => _applicationDbContext.DeliveryMethods.Include(x => x.PaymentMethods).ThenInclude(p => p.PaymentMethod);

        public void SaveDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            try
            {
                if (deliveryMethod.DeliveryMethodId == 0)
                {
                    Log.Information($"Adding new delivery method: {deliveryMethod.Name}");
                    _applicationDbContext.DeliveryMethods.Add(deliveryMethod);
                }
                else
                {
                    Log.Information($"Updating existing delivery method: {deliveryMethod.Name}");

                    var deliveryMethodEntry = _applicationDbContext.DeliveryMethods.FirstOrDefault(x => x.DeliveryMethodId == deliveryMethod.DeliveryMethodId);
                    
                    if (deliveryMethodEntry != null)
                    {
                        deliveryMethodEntry.Name = deliveryMethod.Name;
                        deliveryMethodEntry.PaymentMethods = new List<DeliveryPaymentMethod>();

                        foreach (var paymentMethod in deliveryMethod.PaymentMethods.ToList())
                        {
                            deliveryMethodEntry.PaymentMethods.Add(paymentMethod);
                        }
                    }
                }

                _applicationDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Error($"Error while adding/updating author...");
                Log.Error(e.Message);
                Log.Error(e.StackTrace);
                Console.WriteLine(e);
            }
        }
    }
}