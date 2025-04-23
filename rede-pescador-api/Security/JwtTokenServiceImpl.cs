using Microsoft.IdentityModel.Tokens;
using rede_pescador_api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtTokenServiceImpl : IJwtTokenService
{
    private readonly IConfiguration _config;

    public JwtTokenServiceImpl(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        var key = Encoding.ASCII.GetBytes("8d99dX1fZPqfD56Tkcj3pZdTdzdfsdfsdfdffwefsdcsrggwsfdsa");
        var Skey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(Skey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.Phone.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };

        var token = new JwtSecurityToken(
               issuer: "RedePescadorAPI",
               audience: "RedePescadorClient",
               claims: claims,
               expires: DateTime.UtcNow.AddHours(1),
               signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);


    }



}
