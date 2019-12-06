using System.Linq;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public interface IPaymentMethodRepository
    {
        IQueryable<PaymentMethod> PaymentMethods { get; }
        void SavePaymentMethod(PaymentMethod paymentMethod);
    }
}