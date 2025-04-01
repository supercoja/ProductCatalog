using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Domain;

namespace webAPI;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderQueue _orderQueue;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(
        IOrderRepository orderRepository,
        IOrderQueue orderQueue,
        ILogger<OrdersController> logger)
    {
        _orderRepository = orderRepository;
        _orderQueue = orderQueue;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderRepository.GetOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(string id)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(Order order)
    {
        // Save the order to the repository
        var orderId = await _orderRepository.CreateOrderAsync(order);

        // Add to processing queue
        _orderQueue.EnqueueOrder(order);
        _logger.LogInformation("Order {OrderId} added to processing queue", orderId);

        return CreatedAtAction(nameof(GetOrder), new { id = orderId }, order);
    }
}