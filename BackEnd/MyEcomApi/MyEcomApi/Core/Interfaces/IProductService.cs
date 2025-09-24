using MyEcomApi.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyEcomApi.Core.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> CreateAsync(ProductCreateDto dto);
        Task UpdateAsync(int id, ProductDto dto);
        Task DeleteAsync(int id);
        Task<ProductDto> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllAsync();
    }
}
