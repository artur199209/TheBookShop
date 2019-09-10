using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBookShop.Models
{
    public class EFPaymentRepository : IPaymentRepository
    {
        private ApplicationDbContext applicationDbContext;

        public EFPaymentRepository(ApplicationDbContext _applicationDbContext)
        {
            applicationDbContext = _applicationDbContext;
        }

        public IQueryable<Payment> Payments => applicationDbContext.Payments;

        public void SavePayment(Payment payment)
        {
            try
            {
                if (payment.PaymentId == 0)
                {
                    applicationDbContext.Payments.Add(payment);
                }
                else
                {
                    var paymentEntry = applicationDbContext.Payments
                        .FirstOrDefault(x => x.PaymentId == payment.PaymentId);

                    if (paymentEntry != null)
                    {
                        paymentEntry.Amount = payment.Amount;
                        paymentEntry.Customer = payment.Customer;
                        paymentEntry.PaymentDate = payment.PaymentDate;
                    }
                }

                applicationDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
    }
}