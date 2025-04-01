namespace webAPI;

public class OrderProcessingService : BackgroundService
{
    private readonly IOrderQueue _orderQueue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrderProcessingService> _logger;
        
    public OrderProcessingService(
        IOrderQueue orderQueue,
        IServiceProvider serviceProvider,
        ILogger<OrderProcessingService> logger)
    {
        _orderQueue = orderQueue;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
        
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Order Processing Service started");
            
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_orderQueue.TryDequeueOrder(out var order))
            {
                _logger.LogInformation("Dequeued order {OrderId} for processing", order.Id);
                    
                using (var scope = _serviceProvider.CreateScope())
                {
                    var orderProcessor = scope.ServiceProvider.GetRequiredService<IOrderProcessor>();
                    await orderProcessor.ProcessOrderAsync(order);
                }
            }
                
            // Delay to demonstrate 
            await Task.Delay(100, stoppingToken);
        }
            
        _logger.LogInformation("Order Processing Service stopped");
    }
}