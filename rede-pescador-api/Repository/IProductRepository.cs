using rede_pescador_api.Models;

namespace rede_pescador_api.Repository
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(long id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(long id);
    }
}
