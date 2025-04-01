using System.Collections.Concurrent;
using ProductCatalog.Domain;

namespace webAPI;

public class InMemoryOrderQueue : IOrderQueue
{
    private readonly ConcurrentQueue<Order> _orderQueue = new ConcurrentQueue<Order>();
        
    public void EnqueueOrder(Order order)
    {
        _orderQueue.Enqueue(order);
    }
        
    public bool TryDequeueOrder(out Order order)
    {
        return _orderQueue.TryDequeue(out order);
    }
        
    public int Count => _orderQueue.Count;
}