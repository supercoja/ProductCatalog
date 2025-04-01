using ProductCatalog.Domain;

namespace webAPI;

public interface IOrderQueue
{
    void EnqueueOrder(Order order);
    bool TryDequeueOrder(out Order order);
    int Count { get; }
}