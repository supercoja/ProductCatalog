namespace ProductCatalog.Domain.Tests;
using Domain;
using FluentAssertions;

public class DomainTests
{
    [Fact]
    public void Product_CreateProduct_With_Valid_Data_Should_Create()
    {
        // Arrange
        var categoryResult = Category.Create("Electronics");
        categoryResult.IsSuccess.Should().BeTrue();
        var category = categoryResult.Value;

        var name = "Laptop";
        var price = 1500m;

        // Act
        var result = Product.CreateProduct(name, price, category);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be(name);
        result.Value.Price.Should().Be(price);
        result.Value.Category.Should().Be(category);
    }

    [Fact]
    public void Product_CreateProduct_With_Invalid_Data_ShouldFail()
    {
        // Arrange
        var category = Category.Create("Electronics").Value;

        // Act
        var result = Product.CreateProduct(string.Empty, 1500m, category);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Product name cannot be empty.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Product_CreateProduct_With_Invalid_Price_ShouldFail(decimal invalidPrice)
    {
        // Arrange
        var category = Category.Create("Electronics").Value;

        // Act
        var result = Product.CreateProduct("NoteBook", invalidPrice, category);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Price must be greater than zero.");
    }

    [Fact]
    public void Product_CreateProduct_With_Invalid_Category_ShouldFail()
    {
        // Arrange
        var name = "Laptop";
        var price = 1500m;

        // Act
        var result = Product.CreateProduct(name, price, null);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Category is required.");
    }

    [Fact]
    public void Category_Create_With_Valid_Data_ShouldCreate()
    {
        // Arrange
        var name = "Electronics";

        // Act
        var result = Category.Create(name);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be(name);
    }

    [Fact]
    public void Category_Create_With_Invalid_Data_ShouldFail()
    {
        // Act
        var result = Category.Create(string.Empty);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Category name cannot be empty.");
    }
}