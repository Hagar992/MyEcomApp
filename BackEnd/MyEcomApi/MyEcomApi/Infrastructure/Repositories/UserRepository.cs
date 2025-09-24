using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyEcomApi.Core.Entities;
using MyEcomApi.Core.Interfaces;
using MyEcomApi.Infrastructure.Data;

namespace MyEcomApi.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
        }
    }
}
