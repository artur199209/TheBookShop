using System.Linq;

namespace TheBookShop.Models
{
    interface IPaymentRepository
    {
        IQueryable<Payment> Payments { get; }
        void SavePayment(Payment payment);
    }
}