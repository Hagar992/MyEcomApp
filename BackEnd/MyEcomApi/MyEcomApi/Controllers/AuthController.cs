using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyEcomApi.Core.DTOs;
using MyEcomApi.Core.Entities;
using MyEcomApi.Core.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEcomApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public AuthController(IUserRepository userRepo, ITokenService tokenService, IConfiguration config)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _config = config;
        }

        [HttpPost("register")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            if (await _userRepo.GetByUserNameAsync(dto.UserName) != null) return BadRequest("UserName exists");
            if (await _userRepo.GetByEmailAsync(dto.Email) != null) return BadRequest("Email exists");

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                LastLogin = null
            };

            await _userRepo.AddAsync(user);
            // احفظ التغييرات — تأكدي إن UserRepository لا تنفّذ SaveChangesAsync داخلها
            // يمكنك استخدام AppDbContext مباشرة هنا، أو UserService يفعل هذا
            return Ok(new { message = "Registered" });
        }

        [HttpPost("login")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var user = await _userRepo.GetByUserNameAsync(dto.UserName);
            if (user == null) return Unauthorized("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) return Unauthorized("Invalid credentials");

            // توليد access token
            var accessToken = _tokenService.GenerateAccessToken(user);

            // توليد refresh token
            var (refreshToken, refreshTokenHash) = _tokenService.GenerateRefreshToken();
            var jwtId = new JwtSecurityTokenHandler().ReadJwtToken(accessToken).Id;
            var refreshExpiresDays = int.Parse(_config["Jwt:RefreshTokenExpirationDays"]);
            var expires = DateTime.UtcNow.AddDays(refreshExpiresDays);

            await _tokenService.SaveRefreshTokenAsync(user.Id, refreshTokenHash, jwtId, expires);

            return Ok(new { accessToken, refreshToken });
        }

        [HttpPost("refresh")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
        {
            // استخراج الـ Claims من token منتهي (expired)
            var principal = GetPrincipalFromExpiredToken(dto.AccessToken);
            if (principal == null) return BadRequest("Invalid access token");

            var jti = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var uidClaim = principal.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (!int.TryParse(uidClaim, out var userId)) return BadRequest("Invalid token claims");

            var matched = await _tokenService.GetValidRefreshTokenAsync(userId, dto.RefreshToken);
            if (matched == null) return Unauthorized("Invalid refresh token");

            if (matched.Revoked != null || matched.Expires <= DateTime.UtcNow) return Unauthorized("Refresh token revoked or expired");
            if (!string.IsNullOrEmpty(matched.JwtId) && !string.IsNullOrEmpty(jti) && matched.JwtId != jti)
            {
                return Unauthorized("Token mismatch");
            }

            // إنشاء access + refresh token جديد
            var user = await _userRepo.GetByIdAsync(userId);
            var newAccess = _tokenService.GenerateAccessToken(user);
            var (newRefresh, newRefreshHash) = _tokenService.GenerateRefreshToken();
            var newJwtId = new JwtSecurityTokenHandler().ReadJwtToken(newAccess).Id;
            var refreshExpiresDays = int.Parse(_config["Jwt:RefreshTokenExpirationDays"]);
            var newExpires = DateTime.UtcNow.AddDays(refreshExpiresDays);

            await _tokenService.SaveRefreshTokenAsync(userId, newRefreshHash, newJwtId, newExpires);
            await _tokenService.RevokeRefreshTokenAsync(matched, newRefreshHash);

            return Ok(new { accessToken = newAccess, refreshToken = newRefresh });
        }

        private System.Security.Claims.ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                ValidateLifetime = false // نتجاهل الصلاحية حتى لو انتهى الوقت
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                if (!(securityToken is JwtSecurityToken jwt) || !jwt.Header.Alg.Equals(Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
