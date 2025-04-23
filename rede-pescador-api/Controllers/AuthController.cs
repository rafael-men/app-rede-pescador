using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rede_pescador_api.Dto;
using rede_pescador_api.Models;
using rede_pescador_api.Services;

namespace rede_pescador_api.Controllers
{
    [ApiController]
    [Route("rede-pescador")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("registro")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var user = await _userService.RegisterAsync(dto);
                return Ok(new
                {
                    message = "Usuário registrado com sucesso",
                    user = new
                    {
                        user.Id,
                        user.Name,
                        user.Email,
                        user.Phone,
                        user.Role
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var token = await _userService.LoginAsync(dto);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("login-fone")]
        public async Task<IActionResult> LoginByPhone([FromBody] LoginPhoneDto dto)
        {
            try
            {
                var token = await _userService.LoginByPhoneAsync(dto);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            try
            {
               
                var userPrincipal = User;

               
                var userIdClaim = userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new { error = "Usuário não autenticado." });
                }

                
                if (!long.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new { error = "ID do usuário inválido." });
                }

                
                var user = await _userService.GetUserFromTokenAsync(userPrincipal);

                if (user == null)
                {
                    return NotFound(new { error = "Usuário não encontrado." });
                }

                
                return Ok(new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Phone,
                    user.Role
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

