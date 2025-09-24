using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyEcomApi.Core.Entities;
using MyEcomApi.Core.Interfaces;
using MyEcomApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MyEcomApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public TokenService(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        public string GenerateAccessToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:AccessTokenExpirationMinutes"])),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (string token, string tokenHash) GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var refreshToken = Convert.ToBase64String(randomBytes);

            var refreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken);

            return (refreshToken, refreshTokenHash);
        }

        public async Task SaveRefreshTokenAsync(int userId, string refreshTokenHash, string jwtId, DateTime expires)
        {
            var rt = new RefreshToken
            {
                TokenHash = refreshTokenHash,
                JwtId = jwtId,
                UserId = userId,
                Expires = expires,
                Created = DateTime.UtcNow
            };

            await _context.RefreshTokens.AddAsync(rt);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetRefreshTokenByIdAsync(int id)
        {
            return await _context.RefreshTokens.FindAsync(id);
        }

        public async Task<RefreshToken> GetValidRefreshTokenAsync(int userId, string refreshTokenPlainText)
        {
            var candidates = await _context.RefreshTokens
                .Where(t => t.UserId == userId && t.Revoked == null && t.Expires > DateTime.UtcNow)
                .ToListAsync();

            foreach (var c in candidates)
            {
                if (BCrypt.Net.BCrypt.Verify(refreshTokenPlainText, c.TokenHash))
                {
                    return c;
                }
            }

            return null;
        }

        public async Task RevokeRefreshTokenAsync(RefreshToken token, string replacedByHash = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.ReplacedByToken = replacedByHash;
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }
    }
}
