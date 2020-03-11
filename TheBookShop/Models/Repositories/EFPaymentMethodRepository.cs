using System;
using System.Linq;
using Serilog;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public class EFPaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EFPaymentMethodRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IQueryable<PaymentMethod> PaymentMethods => _applicationDbContext.PaymentMethods;

        public void SavePaymentMethod(PaymentMethod paymentMethod)
        {
            try
            {
                if (paymentMethod.PaymentMethodId == 0)
                {
                    Log.Information("Adding new payment method...");
                    _applicationDbContext.PaymentMethods.Add(paymentMethod);
                }
                else
                {
                    Log.Information("Updating existing payment method...");

                    var paymentMethodEntry =
                        _applicationDbContext.PaymentMethods.FirstOrDefault(x =>
                            x.PaymentMethodId == paymentMethod.PaymentMethodId);

                    if (paymentMethodEntry != null)
                    {
                        paymentMethodEntry.Name = paymentMethod.Name;
                        paymentMethodEntry.Price = paymentMethod.Price;
                    }
                }

                _applicationDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Log.Error($"Error while saving order...");
                Log.Error(e.Message);
                Log.Error(e.StackTrace);
                Console.WriteLine(e);
            }
        }
    }
}