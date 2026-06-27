using Microsoft.EntityFrameworkCore;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Domain.Entities;
using MiniInventory.Infrastructure.Data;

namespace MiniInventory.Infrastructure.Repositories;

public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(AppDbContext context) : base(context) { }

    public async Task<int> CountAsync() => await DbSet.CountAsync();
}
