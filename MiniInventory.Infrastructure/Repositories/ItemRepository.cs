using Microsoft.EntityFrameworkCore;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Domain.Entities;
using MiniInventory.Infrastructure.Data;

namespace MiniInventory.Infrastructure.Repositories;

public class ItemRepository : GenericRepository<Item>, IItemRepository
{
    public ItemRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Item>> GetAllWithRelationsAsync()
        => await DbSet.AsNoTracking()
            .Include(i => i.Category)
            .Include(i => i.Supplier)
            .OrderBy(i => i.ItemName)
            .ToListAsync();

    public async Task<Item?> GetByIdWithRelationsAsync(int id)
        => await DbSet.AsNoTracking()
            .Include(i => i.Category)
            .Include(i => i.Supplier)
            .FirstOrDefaultAsync(i => i.ItemId == id);

    public async Task<IReadOnlyList<Item>> SearchAsync(string keyword)
    {
        keyword = (keyword ?? string.Empty).Trim();
        return await DbSet.AsNoTracking()
            .Include(i => i.Category)
            .Include(i => i.Supplier)
            .Where(i => string.IsNullOrEmpty(keyword)
                || i.ItemName.Contains(keyword)
                || i.ItemCode.Contains(keyword)
                || (i.Barcode != null && i.Barcode.Contains(keyword)))
            .OrderBy(i => i.ItemName)
            .ToListAsync();
    }

    public async Task<bool> ItemCodeExistsAsync(string itemCode, int? excludeId = null)
        => await DbSet.AnyAsync(i => i.ItemCode == itemCode
            && (excludeId == null || i.ItemId != excludeId));

    public async Task<int> CountAsync() => await DbSet.CountAsync();
}
