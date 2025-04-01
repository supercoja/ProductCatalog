namespace ProductCatalog.Domain;

public class Product
{
    protected Product(Guid id, string name, decimal price, Category category)
    {
        Id = id;
        Name = name;
        Price = price;
        Category = category;
    }
    
    public static Result<Product> CreateProduct(string name, decimal price, Category category)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Fail<Product>("Product name cannot be empty.");

        if (price <= 0)
            return Result.Fail<Product>("Price must be greater than zero.");

        if (category is null)
            return Result.Fail<Product>("Category is required.");

        return Result.Ok(new Product(Guid.NewGuid(), name, price, category));
    }
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price {get; private set;}
    public Category Category { get; private set; }
    
}