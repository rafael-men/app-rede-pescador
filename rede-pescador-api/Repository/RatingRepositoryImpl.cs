namespace rede_pescador_api.Repository
{
    using Microsoft.EntityFrameworkCore;
    using rede_pescador_api.Data;
    using rede_pescador_api.Models.rede_pescador_api.Models;

    public class RatingRepositoryImpl : IRatingRepository
    {
        private readonly AppDbContext _context;

        public RatingRepositoryImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Rating rating)
        {
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
        }

        public async Task<Rating?> GetByOrderIdAsync(long orderId)
        {
            return await _context.Ratings
                .Include(r => r.Order)
                .FirstOrDefaultAsync(r => r.OrderId == orderId);
        }

        public async Task<IEnumerable<Rating>> GetAllByUserIdAsync(long userId)
        {
            return await _context.Ratings
                .Include(r => r.Order)
                .Where(r => r.Order.BuyerId == userId)
                .ToListAsync();
        }
    }

}
