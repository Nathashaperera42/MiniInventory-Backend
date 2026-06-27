using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Infrastructure.Data;

namespace MiniInventory.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<T> DbSet;

    public GenericRepository(AppDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
        => await DbSet.AsNoTracking().ToListAsync();

    public async Task<T?> GetByIdAsync(int id)
        => await DbSet.FindAsync(id);

    public async Task<T> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        return entity;
    }

    public Task UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        => await DbSet.AnyAsync(predicate);

    public async Task<int> SaveChangesAsync()
        => await Context.SaveChangesAsync();
}
