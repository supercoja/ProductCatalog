using Microsoft.Extensions.Logging;
using ProductCatalog.Domain;

namespace ProductCatalog.Data.Fake;

public class FakeOrderRepository : IOrderRepository
{
    private readonly Dictionary<string, Order> _orders = new Dictionary<string, Order>();
    private readonly ILogger<FakeOrderRepository> _logger;

    public FakeOrderRepository(ILogger<FakeOrderRepository> logger)
    {
        _logger = logger;
    }

    public Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return Task.FromResult<IEnumerable<Order>>(_orders.Values);
    }

    public Task<Order> GetOrderByIdAsync(string id)
    {
        if (_orders.TryGetValue(id, out var order))
        {
            return Task.FromResult(order);
        }
        return Task.FromResult<Order>(null);
    }

    public Task<string> CreateOrderAsync(Order order)
    {
        _orders[order.Id] = order;
        _logger.LogInformation("Order created: {OrderId}", order.Id);
        return Task.FromResult(order.Id);
    }

    public Task UpdateOrderAsync(Order order)
    {
        _orders[order.Id] = order;
        _logger.LogInformation("Order updated: {OrderId}", order.Id);
        return Task.CompletedTask;
    }
}
