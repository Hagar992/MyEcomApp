using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using MyEcomApi.Infrastructure.Data;
using MyEcomApi.Infrastructure.Repositories;
using MyEcomApi.Services;
using MyEcomApi.Core.DTOs;

public class ProductServiceTests
{
    [Fact]
    public async Task CreateAsync_CallsRepoAndSaves()
    {
        // Arrange: InMemory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        await using var context = new AppDbContext(options);
        var repo = new ProductRepository(context);
        var service = new ProductService(repo, context);

        var dto = new ProductCreateDto
        {
            ProductCode = "P01",
            Name = "X",
            Category = "C",
            Price = 10,
            MinQuantity = 1,
            DiscountRate = 0
        };

        // Act: call service
        var result = await service.CreateAsync(dto);

        // Assert
        Assert.Equal("P01", result.ProductCode);
        Assert.Equal("X", result.Name);
        Assert.Equal("C", result.Category);
    }
}
