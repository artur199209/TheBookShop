using System.Linq;
using Microsoft.EntityFrameworkCore;
using TheBookShop.Models.DataModels;

namespace TheBookShop.Models.Repositories
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public EFOrderRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public IQueryable<Order> Orders => _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.DeliveryAddress)
            .Include(o => o.Payment)
            .Include(o => o.PaymentMethod)
            .ThenInclude(p => p.DeliveryMethods)
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product)
            .ThenInclude(p => p.Author);

        public void SaveOrder(Order order)
        {
            _context.AttachRange(order.Lines.Select(l => l.Product));
            _context.Attach(order.DeliveryMethod);
            _context.AttachRange(order.DeliveryMethod.PaymentMethods);
            _context.Attach(order.PaymentMethod);

            if (order.OrderId == 0)
            {
                _context.Orders.Add(order);
            }
            _context.SaveChanges();
        }
    }
}