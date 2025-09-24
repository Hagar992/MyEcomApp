using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyEcomApi.Core.Entities;
using MyEcomApi.Core.Interfaces;
using MyEcomApi.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyEcomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _context;

        public ProductsController(IProductRepository productRepository, AppDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            // تأكدي من unique product code إلخ
            await _productRepository.AddAsync(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            _productRepository.UpdateAsync(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _productRepository.DeleteAsync(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
