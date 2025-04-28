using Microsoft.EntityFrameworkCore;
using rede_pescador_api.Models;
using rede_pescador_api.Models.rede_pescador_api.Models;

namespace rede_pescador_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
