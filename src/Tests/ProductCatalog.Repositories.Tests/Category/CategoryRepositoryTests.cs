using Moq;
using FluentAssertions;
using ProductCatalog.Domain;

namespace ProductCatalog.Repositories.Tests;

public class CategoryRepositoryTests 
{
    private readonly Mock<ICategoryRepository> _mockRepository = new();

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategory_WhenExists()
    {
        var category = Category.Create("Electronics").Value;
        _mockRepository.Setup(repo => repo.GetByIdAsync(category.Id)).ReturnsAsync(category);

        var result = await _mockRepository.Object.GetByIdAsync(category.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(category.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Category?)null);

        var result = await _mockRepository.Object.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnCategory_WhenExists()
    {
        var category = Category.Create("Electronics").Value;
        _mockRepository.Setup(repo => repo.GetByNameAsync("Electronics")).ReturnsAsync(category);

        var result = await _mockRepository.Object.GetByNameAsync("Electronics");

        result.Should().NotBeNull();
        result!.Name.Should().Be("Electronics");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        var categories = new List<Category>
        {
            Category.Create("Cellphone").Value,
            Category.Create("Headset").Value
        };
        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

        var result = await _mockRepository.Object.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task AddAsync_ShouldBeCalledOnce()
    {
        var category = Category.Create("Notebooks").Value;

        await _mockRepository.Object.AddAsync(category);

        _mockRepository.Verify(repo => repo.AddAsync(category), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldBeCalledOnce()
    {
        var category = Category.Create("Cellphone").Value;

        await _mockRepository.Object.UpdateAsync(category);

        _mockRepository.Verify(repo => repo.UpdateAsync(category), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldBeCalledOnce()
    {
        var categoryId = Guid.NewGuid();

        await _mockRepository.Object.DeleteAsync(categoryId);

        _mockRepository.Verify(repo => repo.DeleteAsync(categoryId), Times.Once);
    }
}