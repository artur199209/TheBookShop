using System.Linq;

namespace TheBookShop.Models
{
    public interface IPaymentRepository
    {
        IQueryable<Payment> Payments { get; }
        void SavePayment(Payment payment);
    }
}