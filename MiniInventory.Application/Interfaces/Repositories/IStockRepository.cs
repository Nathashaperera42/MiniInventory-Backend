using MiniInventory.Application.DTOs.Stock;
using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Interfaces.Repositories;

public interface IStockRepository
{
    Task<StockIn> AddStockInAsync(StockIn stockIn);
    Task<StockOut> AddStockOutAsync(StockOut stockOut);
    Task<int> CurrentBalanceAsync(int itemId);
    Task<IReadOnlyList<StockBalanceDto>> GetStockBalanceAsync();
    Task<IReadOnlyList<StockBalanceDto>> GetLowStockAsync();
    Task<int> SaveChangesAsync();
}
