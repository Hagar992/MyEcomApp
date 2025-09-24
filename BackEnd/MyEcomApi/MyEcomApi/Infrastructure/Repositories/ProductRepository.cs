using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyEcomApi.Core.Entities;
using MyEcomApi.Core.Interfaces;
using MyEcomApi.Infrastructure.Data;

namespace MyEcomApi.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(Product product) => await _context.Products.AddAsync(product);

        public async Task DeleteAsync(Product product) => _context.Products.Remove(product);

        public async Task<List<Product>> GetAllAsync() => await _context.Products.ToListAsync();

        public async Task<Product> GetByCodeAsync(string productCode) =>
            await _context.Products.FirstOrDefaultAsync(p => p.ProductCode == productCode);

        public async Task<Product> GetByIdAsync(int id) => await _context.Products.FindAsync(id);

        public async Task UpdateAsync(Product product) => _context.Products.Update(product);
    }
}
