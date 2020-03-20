using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serilog;
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
            .Include(o => o.DeliveryPaymentMethod)
            .ThenInclude(o => o.PaymentMethod)
            .Include(o => o.DeliveryPaymentMethod)
            .ThenInclude(o => o.DeliveryMethod)
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product)
            .ThenInclude(c => c.Category)
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product)
            .ThenInclude(p => p.Author);

        public void SaveOrder(Order order)
        {
            try
            {
                _context.AttachRange(order.Lines.Select(l => l.Product));
                _context.Attach(order.DeliveryPaymentMethod);

                if (order.OrderId == 0)
                {
                    _context.Orders.Add(order);
                }
                _context.SaveChanges();
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