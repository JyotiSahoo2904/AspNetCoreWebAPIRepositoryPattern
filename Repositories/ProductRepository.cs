using AspNetCoreWebAPIRepositoryPattern.Data;
using AspNetCoreWebAPIRepositoryPattern.DTOs;
using AspNetCoreWebAPIRepositoryPattern.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace AspNetCoreWebAPIRepositoryPattern.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();

        private readonly AppDbContext _context;

        private readonly IMemoryCache _memoryCache;

        public ProductRepository(AppDbContext context, IMemoryCache memoryCache)
        {
            _context = context;

            _memoryCache = memoryCache;

        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {          
            var ProductDtos = await _context.Products.Where(p => p.Version == "1").Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description=p.Description               

            }).ToArrayAsync();
            return ProductDtos;
        }

        public async Task<IEnumerable<ProductDtoV2>> GetAllProductsV2Async()
        {
            var ProductDtosV2 = await _context.Products.Where(p => p.Version == "2").Select(p => new ProductDtoV2
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description

            }).ToArrayAsync();
            return ProductDtosV2;
        }


        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            ProductDto productDto;

            if (!_memoryCache.TryGetValue(id, out productDto))
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == id);
                if (product == null) return null;

                productDto=  new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description

                };

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                // Save data in cache
                _memoryCache.Set(id, productDto, cacheEntryOptions);


            }
            return productDto;
            
        }



        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            int productCount = _context.Products.Count();
            var product = new Product
            {
                Id = productCount + 1,
                Name = productDto.Name,
                Price = productDto.Price
            };

            _context.Products.Add(product);

            return productDto;
        }

        public async Task UpdateProductAsync(ProductDto productDto)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productDto.Id);
            if (product == null) return;

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null) _products.Remove(product);
        }
    }
}
