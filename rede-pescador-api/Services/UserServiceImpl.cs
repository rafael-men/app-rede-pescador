using BCrypt.Net; 
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
        var existingEmail = await _repository.GetByEmailAsync(dto.Email);
        if (existingEmail != null)
            throw new Exception("Email já está registrado.");

        var existingPhone = await _repository.GetByPhoneAsync(dto.Phone);
        if (existingPhone != null)
            throw new Exception("Telefone já está registrado.");

        var validRoles = new[] { "PESCADOR", "CONSUMIDOR"};
        if (!validRoles.Contains(dto.Role?.ToUpper()))
            throw new Exception("Role inválida. Deve ser PESCADOR ou CONSUMIDOR (escreva em caps lock).");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role.ToUpper(),
            ProfileImageUrl = dto.ImageUrl
        };

        await _repository.AddAsync(user);
        return user;
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _repository.GetByEmailAsync(email);
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
            throw new Exception("Telefone inválido.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new Exception("Senha inválida.");

        return _jwtService.GenerateToken(user);
    }


    public async Task<User> GetUserFromTokenAsync(ClaimsPrincipal userPrincipal)
    {
        var userIdClaim = userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
            throw new Exception("Id do Usuário inválido");

        return await _repository.GetByUserIdAsync(userId);
    }
}