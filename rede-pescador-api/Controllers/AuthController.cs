using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
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
        private readonly IUserRepository _repository;
        private readonly IJwtTokenService _jwtService;

        public AuthController(IUserService userService, IUserRepository userRepository,IJwtTokenService jwtService)
        {
            _userService = userService;
            _repository = userRepository;
            _jwtService = jwtService;
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
                        user.Role,
                        user.ProfileImageUrl
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

       // [HttpGet("login-google")]
       // public IActionResult LoginGoogle()
       // {
          // var redirectUrl = Url.Action("GoogleCallback", "Auth");
          //  var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
          // return Challenge(properties, "Google");
       // }

       // [HttpGet("google-callback")]
       // public async Task<IActionResult> GoogleCallback()
       // {
           // var result = await HttpContext.AuthenticateAsync("Cookies");
           // var claims = result.Principal.Identities.FirstOrDefault()?.Claims;

           // var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
           //  var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
           // var imageUrl = claims?.FirstOrDefault(c => c.Type == "picture")?.Value; 

      
           // var user = await _userService.FindByEmailAsync(email);
        
           // if (user == null)
           // {
             //   return BadRequest(new { message = "Usuário não cadastrado." });
         //   }

         //   var token = _jwtService.GenerateToken(user);
        //    return Ok(new { token });
       // }


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
                    user.ProfileImageUrl,
                    user.Phone,
                    user.Role
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("foto-perfil")]
        [Authorize]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nenhum arquivo enviado.");

            // Recupera o ID do usuário autenticado a partir do JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("Usuário não autenticado.");

            if (!int.TryParse(userIdClaim, out var userId))
                return BadRequest("ID do usuário inválido.");

            var user = await _repository.GetByUserIdAsync(userId);
            if (user == null)
                return NotFound("Usuário não encontrado.");

            // Gera um nome de arquivo único e define o caminho para salvar
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile-images");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            // Salva o arquivo no sistema de arquivos
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Atualiza a URL da imagem de perfil do usuário
            user.ProfileImageUrl = $"/profile-images/{fileName}";
            await _repository.UpdateProfileImageAsync(user);


            return Ok(new
            {
                Message = "Imagem de perfil atualizada com sucesso.",
                ImageUrl = user.ProfileImageUrl
            });
        }
    }
}

