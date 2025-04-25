using System.Collections.Generic;
using System.Threading.Tasks;
using rede_pescador_api.Models.rede_pescador_api.Models;

public interface IRatingRepository
{
    Task AddAsync(Rating rating);
    Task<Rating?> GetByOrderIdAsync(long orderId);
    Task<IEnumerable<Rating>> GetAllByUserIdAsync(long userId); // opcional
}
