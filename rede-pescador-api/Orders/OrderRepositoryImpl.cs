using Microsoft.EntityFrameworkCore;
using rede_pescador_api.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rede_pescador_api.Orders
{
    public class OrderRepositoryImpl : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepositoryImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetByIdAsync(long orderId)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetByUserAsync(long userId)
        {
            return await _context.Orders
                .Where(o => o.BuyerId == userId)
                .ToListAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
