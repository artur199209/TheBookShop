using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public class EFDeliveryMethodRepository : IDeliveryMethodRepository
    {
        private ApplicationDbContext _applicationDbContext;

        public EFDeliveryMethodRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IQueryable<DeliveryMethod> DeliveryMethods => _applicationDbContext.DeliveryMethods.Include(x => x.PaymentMethods);

        public void SaveDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            try
            {
                if (deliveryMethod.DeliveryMethodId == 0)
                {
                    _applicationDbContext.DeliveryMethods.Add(deliveryMethod);
                }
                else
                {
                    var deliveryMethodEntry = _applicationDbContext.DeliveryMethods.FirstOrDefault(x => x.DeliveryMethodId == deliveryMethod.DeliveryMethodId);

                    if (deliveryMethodEntry != null)
                    {
                        deliveryMethodEntry.Name = deliveryMethod.Name;
                    }
                }

                _applicationDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}