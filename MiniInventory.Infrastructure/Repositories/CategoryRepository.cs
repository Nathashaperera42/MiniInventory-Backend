using Microsoft.EntityFrameworkCore;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Domain.Entities;
using MiniInventory.Infrastructure.Data;

namespace MiniInventory.Infrastructure.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Category>> SearchAsync(string keyword)
    {
        keyword = (keyword ?? string.Empty).Trim();
        return await DbSet.AsNoTracking()
            .Where(c => string.IsNullOrEmpty(keyword) || c.CategoryName.Contains(keyword))
            .OrderBy(c => c.CategoryName)
            .ToListAsync();
    }

    public async Task<int> CountAsync() => await DbSet.CountAsync();
}
