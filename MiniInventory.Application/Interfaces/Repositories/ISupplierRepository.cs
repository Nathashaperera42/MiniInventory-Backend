using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Interfaces.Repositories;

public interface ISupplierRepository : IGenericRepository<Supplier>
{
    Task<int> CountAsync();
}
