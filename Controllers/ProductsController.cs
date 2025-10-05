using Asp.Versioning;
using AspNetCoreWebAPIRepositoryPattern.DTOs;
using AspNetCoreWebAPIRepositoryPattern.Filters;
using AspNetCoreWebAPIRepositoryPattern.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AspNetCoreWebAPIRepositoryPattern.Controllers
{
    [ApiController]
    // [Route("api/v1/[controller]")]    
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [TypeFilter(typeof(LogActionFilter))]

    public class ProductsController : ControllerBase
    {
        
        private readonly IProductRepository _repository;
      

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        // GET: api/products
        [HttpGet]
       // [ResponseCache(Duration = 60)]
        [EnableRateLimiting("FixedPolicy")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _repository.GetAllProductsAsync();
            return Ok(products.ToArray());
        }

        // GET: api/productsV2
        [HttpGet("V2")]
       //[ResponseCache(Duration = 60)]
        [EnableRateLimiting("FixedPolicy")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsV2()
        {
            var products = await _repository.GetAllProductsV2Async();
            return Ok(products.ToArray());
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if (product == null) 
                 return NotFound();

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdProduct = await _repository.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody]ProductDto productDto)
        {
            if (id != productDto.Id) return BadRequest();

            await _repository.UpdateProductAsync(productDto);
            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _repository.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
