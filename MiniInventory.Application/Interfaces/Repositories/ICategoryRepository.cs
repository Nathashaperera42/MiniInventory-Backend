using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Interfaces.Repositories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IReadOnlyList<Category>> SearchAsync(string keyword);
    Task<int> CountAsync();
}
