namespace ProductCatalog.Domain;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order> GetOrderByIdAsync(string id);
    Task<string> CreateOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
}