using Microsoft.EntityFrameworkCore;
using MiniInventory.Application.DTOs.Stock;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Domain.Entities;
using MiniInventory.Infrastructure.Data;

namespace MiniInventory.Infrastructure.Repositories;

public class StockRepository : IStockRepository
{
    private readonly AppDbContext _context;

    public StockRepository(AppDbContext context) => _context = context;

    public async Task<StockIn> AddStockInAsync(StockIn stockIn)
    {
        await _context.StockIns.AddAsync(stockIn);
        return stockIn;
    }

    public async Task<StockOut> AddStockOutAsync(StockOut stockOut)
    {
        await _context.StockOuts.AddAsync(stockOut);
        return stockOut;
    }

    public async Task<int> CurrentBalanceAsync(int itemId)
    {
        var stockIn = await _context.StockIns
            .Where(s => s.ItemId == itemId)
            .SumAsync(s => (int?)s.Quantity) ?? 0;

        var stockOut = await _context.StockOuts
            .Where(s => s.ItemId == itemId)
            .SumAsync(s => (int?)s.Quantity) ?? 0;

        return stockIn - stockOut;
    }

    public async Task<IReadOnlyList<StockBalanceDto>> GetStockBalanceAsync()
    {
        // Aggregate stock-in and stock-out per item, then compute balance + status.
        var items = await _context.Items
            .AsNoTracking()
            .Include(i => i.Category)
            .Select(i => new
            {
                i.ItemId,
                i.ItemCode,
                i.ItemName,
                CategoryName = i.Category != null ? i.Category.CategoryName : string.Empty,
                i.ReorderLevel,
                TotalIn = _context.StockIns.Where(s => s.ItemId == i.ItemId)
                            .Sum(s => (int?)s.Quantity) ?? 0,
                TotalOut = _context.StockOuts.Where(s => s.ItemId == i.ItemId)
                            .Sum(s => (int?)s.Quantity) ?? 0
            })
            .ToListAsync();

        return items.Select(i =>
        {
            var balance = i.TotalIn - i.TotalOut;
            return new StockBalanceDto
            {
                ItemId = i.ItemId,
                ItemCode = i.ItemCode,
                ItemName = i.ItemName,
                CategoryName = i.CategoryName,
                TotalStockIn = i.TotalIn,
                TotalStockOut = i.TotalOut,
                CurrentBalance = balance,
                ReorderLevel = i.ReorderLevel,
                StockStatus = ResolveStatus(balance, i.ReorderLevel)
            };
        }).ToList();
    }

    public async Task<IReadOnlyList<StockBalanceDto>> GetLowStockAsync()
    {
        var all = await GetStockBalanceAsync();
        return all.Where(b => b.StockStatus is "Low Stock" or "Out of Stock").ToList();
    }

    public async Task<IReadOnlyList<StockInDto>> GetStockInHistoryAsync()
    {
        var records = await _context.StockIns
            .AsNoTracking()
            .Include(s => s.Item)
            .Include(s => s.Supplier)
            .OrderByDescending(s => s.StockInDate)
            .ToListAsync();

        return records.Select(s => new StockInDto
        {
            StockInId = s.StockInId,
            ItemId = s.ItemId,
            ItemName = s.Item?.ItemName,
            SupplierId = s.SupplierId,
            SupplierName = s.Supplier?.SupplierName,
            Quantity = s.Quantity,
            CostPrice = s.CostPrice,
            StockInDate = s.StockInDate,
            CreatedDate = s.CreatedDate
        }).ToList();
    }

    public async Task<IReadOnlyList<StockOutDto>> GetStockOutHistoryAsync()
    {
        var records = await _context.StockOuts
            .AsNoTracking()
            .Include(s => s.Item)
            .OrderByDescending(s => s.StockOutDate)
            .ToListAsync();

        return records.Select(s => new StockOutDto
        {
            StockOutId = s.StockOutId,
            ItemId = s.ItemId,
            ItemName = s.Item?.ItemName,
            Quantity = s.Quantity,
            Reason = s.Reason.ToString(),
            StockOutDate = s.StockOutDate,
            CreatedDate = s.CreatedDate
        }).ToList();
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    private static string ResolveStatus(int balance, int reorderLevel)
    {
        if (balance <= 0) return "Out of Stock";
        if (balance <= reorderLevel) return "Low Stock";
        return "Good Stock";
    }
}
