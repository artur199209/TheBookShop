using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public class EFPaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EFPaymentRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IQueryable<Payment> Payments => _applicationDbContext.Payments
            .Include(c => c.Customer);

        public void SavePayment(Payment payment)
        {
            try
            {
                if (payment.PaymentId == 0)
                {
                    Log.Information("Adding new payment...");
                    _applicationDbContext.Payments.Add(payment);
                }
                else
                {
                    Log.Information($"Updating exisitng payment {payment.PaymentId}...");

                    var paymentEntry = _applicationDbContext.Payments
                        .FirstOrDefault(x => x.PaymentId == payment.PaymentId);

                    if (paymentEntry != null)
                    {
                        paymentEntry.Amount = payment.Amount;
                        paymentEntry.Customer = payment.Customer;
                        paymentEntry.PaymentDate = payment.PaymentDate;
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