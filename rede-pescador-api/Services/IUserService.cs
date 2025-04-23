using rede_pescador_api.Dto;
using rede_pescador_api.Models;
using System.Security.Claims;

public interface IUserService
{
    Task<User> RegisterAsync(RegisterDto dto);
    Task<string> LoginAsync(LoginDto dto);
    Task<string> LoginByPhoneAsync(LoginPhoneDto dto); 
    Task<User> GetUserFromTokenAsync(ClaimsPrincipal userPrincipal);
}
