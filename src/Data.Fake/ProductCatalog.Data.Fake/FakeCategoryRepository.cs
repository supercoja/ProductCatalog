using System.Collections.Concurrent;

namespace ProductCatalog.Data.Fake;
using Domain;

public class FakeCategoryRepository : ICategoryRepository
{
    private readonly ConcurrentDictionary<Guid, Category> _categories = new();

    public Task<Category?> GetByIdAsync(Guid id) =>
        Task.FromResult(_categories.TryGetValue(id, out var category) ? category : null);

    public Task<IEnumerable<Category>> GetAllAsync() =>
        Task.FromResult(_categories.Values.AsEnumerable());

    public Task<Category?> GetByNameAsync(string name) =>
        Task.FromResult(_categories.Values.FirstOrDefault(c => c.Name == name));

    public Task AddAsync(Category category)
    {
        _categories[category.Id] = category;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Category category)
    {
        if (_categories.ContainsKey(category.Id))
            _categories[category.Id] = category;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _categories.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}