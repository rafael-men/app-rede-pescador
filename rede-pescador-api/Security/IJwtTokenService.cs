using rede_pescador_api.Models;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
