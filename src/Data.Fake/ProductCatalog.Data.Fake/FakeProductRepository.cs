using System.Collections.Concurrent;

namespace ProductCatalog.Data.Fake;
using Domain;

public class FakeProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();

    public Task<Product?> GetByIdAsync(Guid id) =>
        Task.FromResult(_products.TryGetValue(id, out var product) ? product : null);

    public Task<IEnumerable<Product>> GetAllAsync() =>
        Task.FromResult(_products.Values.AsEnumerable());

    public Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId) =>
        Task.FromResult(_products.Values.Where(p => p.Category.Id == categoryId));

    public Task AddAsync(Product product)
    {
        _products[product.Id] = product;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Product product)
    {
        if (_products.ContainsKey(product.Id))
            _products[product.Id] = product;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _products.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}