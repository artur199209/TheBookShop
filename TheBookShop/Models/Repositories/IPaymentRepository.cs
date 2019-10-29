using System.Linq;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public interface IPaymentRepository
    {
        IQueryable<Payment> Payments { get; }
        void SavePayment(Payment payment);
    }
}