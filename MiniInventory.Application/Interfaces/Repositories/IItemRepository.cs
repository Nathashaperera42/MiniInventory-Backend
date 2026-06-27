using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Interfaces.Repositories;

public interface IItemRepository : IGenericRepository<Item>
{
    Task<IReadOnlyList<Item>> GetAllWithRelationsAsync();
    Task<Item?> GetByIdWithRelationsAsync(int id);
    Task<IReadOnlyList<Item>> SearchAsync(string keyword);
    Task<bool> ItemCodeExistsAsync(string itemCode, int? excludeId = null);
    Task<int> CountAsync();
}
