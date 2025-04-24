namespace rede_pescador_api.Orders
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order> GetByIdAsync(long orderId);
        Task<IEnumerable<Order>> GetByUserAsync(long userId);
        Task UpdateAsync(Order order);
    }
}
