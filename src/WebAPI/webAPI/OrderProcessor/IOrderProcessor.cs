using ProductCatalog.Domain;

namespace webAPI;

public interface IOrderProcessor
{ 
    Task ProcessOrderAsync(Order order);
}