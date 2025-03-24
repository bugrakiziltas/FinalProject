using FinalProject.Data;
using FinalProject.Entities;
using FinalProject.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddOrder(Order order)
        {
            _context.Orders.Add(order);
            if (_context.SaveChanges() > 0) return true;
            return false;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(Guid id)
        {
            return await _context.Orders.Where(x => x.IdentityUserId == id).Include(x=>x.BuyedProducts).ToListAsync();
        }
    }
}
