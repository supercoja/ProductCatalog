namespace webAPI;

public class ProductDto
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
}