using System.Linq.Expressions;

namespace MiniInventory.Application.Interfaces.Repositories;

/// <summary>
/// Generic repository contract shared by all entity repositories.
/// Keeps data-access concerns out of the service and controller layers.
/// </summary>
public interface IGenericRepository<T> where T : class
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<int> SaveChangesAsync();
}
