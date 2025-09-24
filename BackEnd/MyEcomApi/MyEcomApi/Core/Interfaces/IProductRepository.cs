using System.Collections.Generic;
using System.Threading.Tasks;
using MyEcomApi.Core.Entities;

namespace MyEcomApi.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> GetByCodeAsync(string productCode);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}
