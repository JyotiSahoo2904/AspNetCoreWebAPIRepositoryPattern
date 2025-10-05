using AspNetCoreWebAPIRepositoryPattern.Data;
using AspNetCoreWebAPIRepositoryPattern.DTOs;
using AspNetCoreWebAPIRepositoryPattern.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AspNetCoreWebAPIRepositoryPattern.Repositories
{
    public class ProductRepositoryV2 :IProductRepositoryV2
    {
        private readonly List<Product> _products = new();

        private readonly AppDbContext _context;

        private readonly IMemoryCache _memoryCache;

        public ProductRepositoryV2(AppDbContext context)
        {
            _context = context;        

        }
        public async Task<IEnumerable<ProductDto>> GetAllProductsV2Async()
        {
            var ProductDtos = await _context.ProductsV2.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description

            }).ToArrayAsync();
            return ProductDtos;
        }
    }
}
