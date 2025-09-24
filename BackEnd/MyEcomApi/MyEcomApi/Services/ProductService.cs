using MyEcomApi.Core.Interfaces;
using MyEcomApi.Core.DTOs;
using MyEcomApi.Core.Entities;
using MyEcomApi.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyEcomApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _context;

        public ProductService(IProductRepository productRepository, AppDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<ProductDto> CreateAsync(ProductCreateDto dto)
        {
           
            var existing = await _productRepository.GetByCodeAsync(dto.ProductCode);
            if (existing != null) throw new Exception("Product code already exists");

            var product = new Product
            {
                ProductCode = dto.ProductCode,
                Name = dto.Name,
                Category = dto.Category,
                ImagePath = dto.ImagePath,
                Price = dto.Price,
                MinQuantity = dto.MinQuantity,
                DiscountRate = dto.DiscountRate
            };

            await _productRepository.AddAsync(product);
            await _context.SaveChangesAsync();

            // تحويل Entity ل DTO
            return new ProductDto
            {
                Id = product.Id,
                ProductCode = product.ProductCode,
                Name = product.Name,
                Category = product.Category,
                ImagePath = product.ImagePath,
                Price = product.Price,
                MinQuantity = product.MinQuantity,
                DiscountRate = product.DiscountRate
            };
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null) throw new Exception("Product not found");
            await _productRepository.DeleteAsync(existing);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var list = await _productRepository.GetAllAsync();
            var result = new List<ProductDto>();
            foreach (var p in list)
            {
                result.Add(new ProductDto
                {
                    Id = p.Id,
                    ProductCode = p.ProductCode,
                    Name = p.Name,
                    Category = p.Category,
                    ImagePath = p.ImagePath,
                    Price = p.Price,
                    MinQuantity = p.MinQuantity,
                    DiscountRate = p.DiscountRate
                });
            }
            return result;
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var p = await _productRepository.GetByIdAsync(id);
            if (p == null) return null;
            return new ProductDto
            {
                Id = p.Id,
                ProductCode = p.ProductCode,
                Name = p.Name,
                Category = p.Category,
                ImagePath = p.ImagePath,
                Price = p.Price,
                MinQuantity = p.MinQuantity,
                DiscountRate = p.DiscountRate
            };
        }

        public async Task UpdateAsync(int id, ProductDto dto)
        {
            if (id != dto.Id) throw new Exception("Id mismatch");
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null) throw new Exception("Product not found");

            // تحديث الحقول
            existing.Name = dto.Name;
            existing.Category = dto.Category;
            existing.Price = dto.Price;
            existing.MinQuantity = dto.MinQuantity;
            existing.DiscountRate = dto.DiscountRate;
            existing.ImagePath = dto.ImagePath;

            _productRepository.UpdateAsync(existing);
            await _context.SaveChangesAsync();
        }
    }
}
