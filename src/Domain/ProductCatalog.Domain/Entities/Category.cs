namespace ProductCatalog.Domain;

public class Category 
{
    protected Category(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Result<Category> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Fail<Category>("Category name cannot be empty.");

        return Result.Ok(new Category(Guid.NewGuid(),name));
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
}