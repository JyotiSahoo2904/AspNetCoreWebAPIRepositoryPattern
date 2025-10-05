using AspNetCoreWebAPIRepositoryPattern.DTOs;

namespace AspNetCoreWebAPIRepositoryPattern.Repositories
{
    public interface IProductRepositoryV2
    {
        Task<IEnumerable<ProductDto>> GetAllProductsV2Async();
    }
}
