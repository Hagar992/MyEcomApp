using System.Threading.Tasks;
using MyEcomApi.Core.DTOs;
using MyEcomApi.Core.Entities;

namespace MyEcomApi.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterAsync(UserRegisterDto dto);
        Task<(string accessToken, string refreshToken)> LoginAsync(UserLoginDto dto);
        Task<User> GetByIdAsync(int id);
        Task UpdateLastLoginAsync(int userId);
    }
}
