using BCrypt.Net; // ✅ Adicionado
using rede_pescador_api.Dto;
using rede_pescador_api.Models;
using System.Security.Claims;

public class UserServiceImpl : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IJwtTokenService _jwtService;

    public UserServiceImpl(IUserRepository repository, IJwtTokenService jwtService)
    {
        _repository = repository;
        _jwtService = jwtService;
    }

    public async Task<User> RegisterAsync(RegisterDto dto)
    {
        var existing = await _repository.GetByEmailAsync(dto.Email);
        if (existing != null)
            throw new Exception("Email already registered.");

        var validRoles = new[] { "PESCADOR", "CONSUMIDOR", "ESTABELECIMENTO" };
        if (!validRoles.Contains(dto.Role?.ToUpper()))
            throw new Exception("Role inválida. Deve ser PESCADOR, CONSUMIDOR ou ESTABELECIMENTO.");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role.ToUpper() 
        };

        await _repository.AddAsync(user);
        return user;
    }


    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _repository.GetByEmailAsync(dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new Exception("Credenciais Inválidas");

        return _jwtService.GenerateToken(user);
    }

    public async Task<string> LoginByPhoneAsync(LoginPhoneDto dto)
    {
        var user = await _repository.GetByPhoneAsync(dto.Phone);
        if (user == null)
            throw new Exception("Telefone Inválido");

        return _jwtService.GenerateToken(user);
    }

    public async Task<User> GetUserFromTokenAsync(ClaimsPrincipal userPrincipal)
    {
        var userIdClaim = userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
            throw new Exception("Id do Usuário inválido");

        return await _repository.GetByIdAsync(userId);
    }
}
