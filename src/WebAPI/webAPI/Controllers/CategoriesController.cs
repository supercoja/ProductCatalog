using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Domain;

namespace webAPI.Controllers;


[Route("api/categories")]
public class CategoriesController : Controller
{
    public CategoriesController(ICategoryRepository categoryRepository, ILogger<ICategoryRepository> logger)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<ICategoryRepository> _logger;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    { 
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            return NotFound("Category not found.");

        return Ok(category);
    }
    
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetByName(string name)
    { 
        var category = await _categoryRepository.GetByNameAsync(name); 
        if (category == null) 
            return NotFound("Category not found.");

        return Ok(category);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    { 
        var categories = await _categoryRepository.GetAllAsync();
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] string name)
    {
            var result = Category.Create(name);
            if (result.IsFailure)
                return BadRequest(result.Error);

            await _categoryRepository.AddAsync(result.Value);
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] string name)
    { 
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) 
            return NotFound("Category not found.");

        var updatedCategory = Category.Create(name);
        if (updatedCategory.IsFailure) 
            return BadRequest(updatedCategory.Error);

        await _categoryRepository.UpdateAsync(updatedCategory.Value);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
         var category = await _categoryRepository.GetByIdAsync(id);
          if (category == null)
              return NotFound("Category not found.");
          await _categoryRepository.DeleteAsync(id);
          return NoContent();
    }
}