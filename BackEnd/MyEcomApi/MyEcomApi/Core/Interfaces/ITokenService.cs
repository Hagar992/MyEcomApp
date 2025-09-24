using System;
using System.Threading.Tasks;
using MyEcomApi.Core.Entities;

namespace MyEcomApi.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        (string token, string tokenHash) GenerateRefreshToken();
        Task SaveRefreshTokenAsync(int userId, string refreshTokenHash, string jwtId, DateTime expires);
        Task<RefreshToken> GetRefreshTokenByIdAsync(int id);
        Task<RefreshToken> GetValidRefreshTokenAsync(int userId, string refreshTokenPlainText);
        Task RevokeRefreshTokenAsync(RefreshToken token, string replacedByHash = null);
    }
}
