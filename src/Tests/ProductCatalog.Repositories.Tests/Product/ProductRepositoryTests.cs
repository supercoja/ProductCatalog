using Moq;
using FluentAssertions;
using ProductCatalog.Domain;

namespace ProductCatalog.Repositories.Tests;

public class ProductRepositoryTests
{
     private readonly Mock<IProductRepository> _mockRepository;

    public ProductRepositoryTests()
    {
        _mockRepository = new Mock<IProductRepository>();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenExists()
    {
        var category = Category.Create("Gaming").Value;
        var product = Product.CreateProduct("Gaming Laptop", 2000m, category).Value;
        _mockRepository.Setup(repo => repo.GetByIdAsync(product.Id)).ReturnsAsync(product);

        var result = await _mockRepository.Object.GetByIdAsync(product.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(product.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product?)null);

        var result = await _mockRepository.Object.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        var category = Category.Create("Tech").Value;
        var products = new List<Product>
        {
            Product.CreateProduct("Laptop", 1000m, category).Value,
            Product.CreateProduct("Smartphone", 800m, category).Value
        };
        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

        var result = await _mockRepository.Object.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByCategoryAsync_ShouldReturnProducts()
    {
        var category = Category.Create("Music").Value;
        var products = new List<Product>
        {
            Product.CreateProduct("Guitar", 500m, category).Value,
            Product.CreateProduct("Piano", 3000m, category).Value
        };

        _mockRepository.Setup(repo => repo.GetByCategoryAsync(category.Id)).ReturnsAsync(products);

        var result = await _mockRepository.Object.GetByCategoryAsync(category.Id);

        result.Should().HaveCount(2);
        result.Should().Contain(p => p.Name == "Guitar");
        result.Should().Contain(p => p.Name == "Piano");
    }

    [Fact]
    public async Task AddAsync_ShouldBeCalledOnce()
    {
        var category = Category.Create("Photography").Value;
        var product = Product.CreateProduct("Camera", 1000m, category).Value;

        await _mockRepository.Object.AddAsync(product);

        _mockRepository.Verify(repo => repo.AddAsync(product), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldBeCalledOnce()
    {
        var category = Category.Create("Furniture").Value;
        var product = Product.CreateProduct("Table", 250m, category).Value;

        await _mockRepository.Object.UpdateAsync(product);

        _mockRepository.Verify(repo => repo.UpdateAsync(product), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldBeCalledOnce()
    {
        var productId = Guid.NewGuid();

        await _mockRepository.Object.DeleteAsync(productId);

        _mockRepository.Verify(repo => repo.DeleteAsync(productId), Times.Once);
    }
}