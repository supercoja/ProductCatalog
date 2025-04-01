namespace ProductCatalog.Domain;

// This is the simpliest possible - for the sake of time to finish
public class Order
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedDate { get; set; }
}