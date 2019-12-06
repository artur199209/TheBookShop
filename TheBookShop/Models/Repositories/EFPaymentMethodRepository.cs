using System;
using System.Linq;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public class EFPaymentMethodRepository : IPaymentMethodRepository
    {
        private ApplicationDbContext _applicationDbContext;

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
                    _applicationDbContext.PaymentMethods.Add(paymentMethod);
                }
                else
                {
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
                Console.WriteLine(e);
                throw;
            }
        }
    }
}