using ProductCatalog.Domain;

namespace webAPI;

public class OrderProcessor : IOrderProcessor
{
    private readonly ILogger<OrderProcessor> _logger;
    private readonly IOrderRepository _orderRepository;
        
    public OrderProcessor(ILogger<OrderProcessor> logger, IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }
        
    public async Task ProcessOrderAsync(Order order)
    {
        _logger.LogInformation("Starting to process order {OrderId}", order.Id);
            
        try
        {
            order.ProcessedDate = DateTime.UtcNow;
                
            await _orderRepository.UpdateOrderAsync(order);
                
            _logger.LogInformation("Order processed: {OrderId}", order.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing order {OrderId}", order.Id);
            throw;
        }
    }
}