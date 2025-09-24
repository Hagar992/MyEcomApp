using MyEcomApi.Core.DTOs;
using MyEcomApi.Core.Entities;
using MyEcomApi.Core.Interfaces;
using MyEcomApi.Infrastructure.Data;
using MyEcomApi.Infrastructure.Repositories;  // أو عبر DI الواجهات
using System;
using System.Threading.Tasks;

namespace MyEcomApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository; // لو محتاج
        private readonly AppDbContext _context;  // أو استخدمي UnitOfWork

        public UserService(IUserRepository userRepository, AppDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<User> RegisterAsync(UserRegisterDto dto)
        {
            // فحص اسم المستخدم و الايميل
            var existsUserName = await _userRepository.GetByUserNameAsync(dto.UserName);
            if (existsUserName != null)
            {
                throw new Exception("UserName already exists");
            }
            var existsEmail = await _userRepository.GetByEmailAsync(dto.Email);
            if (existsEmail != null)
            {
                throw new Exception("Email already exists");
            }

            // تجزئة الباسورد
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                LastLogin = null
            };

            await _userRepository.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<(string accessToken, string refreshToken)> LoginAsync(UserLoginDto dto)
        {
            // تحقق من وجود user
            var user = await _userRepository.GetByUserNameAsync(dto.UserName);
            if (user == null) throw new Exception("Invalid credentials");

            // تحقق من الباسورد
            bool verified = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!verified) throw new Exception("Invalid credentials");

            // تحديث last login
            user.LastLogin = DateTime.UtcNow;
            _userRepository.UpdateAsync(user);
            await _context.SaveChangesAsync();

            // توليد JWT + Refresh Token
            // نكتب هذا الجزء بعد ما نضبط إعدادات JWT
            string accessToken = "";
            string refreshToken = "";

            // return
            return (accessToken, refreshToken);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.LastLogin = DateTime.UtcNow;
                _userRepository.UpdateAsync(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
