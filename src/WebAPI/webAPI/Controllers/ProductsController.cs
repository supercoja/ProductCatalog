using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Domain;

namespace webAPI;

    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepository productRepository, IRepository<Category> categoryRepository, ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found.", id);
                return NotFound("Product not found.");
            }

            return Ok(product);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found.", categoryId);
                return NotFound("Category not found.");
            }

            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto productDto)
        {
            var category = await _categoryRepository.GetByIdAsync(productDto.CategoryId);
            if (category == null)
            {
                _logger.LogWarning("Attempt to create product with non-existing category ID {CategoryId}.", productDto.CategoryId);
                return NotFound("Category not found.");
            }

            var result = Product.CreateProduct(productDto.Name, productDto.Price, category);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("Product creation failed: {Error}", result.Error);
                return BadRequest(result.Error);
            }

            await _productRepository.AddAsync(result.Value);
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductDto productDto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Attempt to update non-existing product with ID {ProductId}.", id);
                return NotFound("Product not found.");
            }

            var category = await _categoryRepository.GetByIdAsync(productDto.CategoryId);
            if (category == null)
            {
                _logger.LogWarning("Attempt to update product with non-existing category ID {CategoryId}.", productDto.CategoryId);
                return NotFound("Category not found.");
            }

            var updatedProduct = Product.CreateProduct(productDto.Name, productDto.Price, category).Value;
            await _productRepository.UpdateAsync(updatedProduct);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Attempt to delete non-existing product with ID {ProductId}.", id);
                return NotFound("Product not found.");
            }

            await _productRepository.DeleteAsync(id);
            return NoContent();
        }
    }
