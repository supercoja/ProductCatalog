using Microsoft.Extensions.Logging;
using ProductCatalog.Domain;
using webAPI.Controllers;

namespace ProductCatalog.WebAPI.Testes;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class CategoryControllerTests
{
    private readonly Mock<ICategoryRepository> _mockRepo;
    private readonly CategoriesController _controller;
    private readonly Mock<ILogger<ICategoryRepository>> _loggerMock;

    public CategoryControllerTests()
    {
        _mockRepo = new Mock<ICategoryRepository>();
        _loggerMock = new Mock<ILogger<ICategoryRepository>>();
        _controller = new CategoriesController(_mockRepo.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            Category.Create("Electronics").Value,
            Category.Create("Books").Value
        };
        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCategories = Assert.IsType<List<Category>>(okResult.Value);
        Assert.Equal(2, returnedCategories.Count);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WithCategory()
    {
        // Arrange
        var category = Category.Create("Furniture").Value;
        _mockRepo.Setup(repo => repo.GetByIdAsync(category.Id)).ReturnsAsync(category);

        // Act
        var result = await _controller.GetById(category.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCategory = Assert.IsType<Category>(okResult.Value);
        Assert.Equal("Furniture", returnedCategory.Name);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_IfCategoryDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Category)null);

        // Act
        var result = await _controller.GetById(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateCategory_ReturnsCreated_WhenSuccessful()
    {
        // Arrange
        var categoryName = "New Category";
        var category = Category.Create(categoryName);
        _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(categoryName);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedCategory = Assert.IsType<Category>(createdResult.Value);
        Assert.Equal(categoryName, returnedCategory.Name);
    }

    [Fact]
    public async Task UpdateCategory_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        var category = Category.Create("Updated Category").Value;
        _mockRepo.Setup(repo => repo.UpdateAsync(category)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(category.Id, category.Name);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCategory_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _mockRepo.Setup(repo => repo.DeleteAsync(categoryId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(categoryId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}