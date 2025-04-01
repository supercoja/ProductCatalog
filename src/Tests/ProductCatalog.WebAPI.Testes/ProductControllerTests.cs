using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webAPI;
using Moq;
using ProductCatalog.Domain;

namespace ProductCatalog.WebAPI.Testes;

public class ProductControllerTests
{
     private readonly Mock<IProductRepository> _mockProductRepo;
    private readonly Mock<IRepository<Category>> _mockCategoryRepo;
    private readonly Mock<ILogger<ProductController>> _mockLogger;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mockProductRepo = new Mock<IProductRepository>();
        _mockCategoryRepo = new Mock<IRepository<Category>>();
        _mockLogger = new Mock<ILogger<ProductController>>();
        _controller = new ProductController(_mockProductRepo.Object, _mockCategoryRepo.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithProducts()
    {
        var products = new List<Product>();
        _mockProductRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(products, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        _mockProductRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product)null);

        var result = await _controller.GetById(Guid.NewGuid());

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenSuccessful()
    {
        var category = Category.Create("Category A").Value;
        var productDto = new ProductDto { Name = "Product A", Price = 10.0m, CategoryId = category.Id };
        _mockCategoryRepo.Setup(repo => repo.GetByIdAsync(category.Id)).ReturnsAsync(category);
        _mockProductRepo.Setup(repo => repo.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        var result = await _controller.Create(productDto);

        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var productDto = new ProductDto { Name = "Updated Product", Price = 20.0m, CategoryId = Guid.NewGuid() };
        _mockProductRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product)null);

        var result = await _controller.Update(Guid.NewGuid(), productDto);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenSuccessful()
    {
        var product = Product.CreateProduct("Product A", 10.0m, Category.Create("Category A").Value).Value;
        _mockProductRepo.Setup(repo => repo.GetByIdAsync(product.Id)).ReturnsAsync(product);
        _mockProductRepo.Setup(repo => repo.DeleteAsync(product.Id)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(product.Id);

        Assert.IsType<NoContentResult>(result);
    }
}