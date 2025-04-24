using rede_pescador_api.Models;

public interface IUserRepository
{
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByPhoneAsync(string phone);
    Task<User> GetByIdAsync(long id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}

