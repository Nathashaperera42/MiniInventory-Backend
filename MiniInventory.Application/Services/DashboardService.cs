using MiniInventory.Application.DTOs.Dashboard;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Application.Interfaces.Services;
using MiniInventory.Shared.CommonResponse;

namespace MiniInventory.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IItemRepository _itemRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly ISupplierRepository _supplierRepo;
    private readonly IStockRepository _stockRepo;

    public DashboardService(
        IItemRepository itemRepo,
        ICategoryRepository categoryRepo,
        ISupplierRepository supplierRepo,
        IStockRepository stockRepo)
    {
        _itemRepo = itemRepo;
        _categoryRepo = categoryRepo;
        _supplierRepo = supplierRepo;
        _stockRepo = stockRepo;
    }

    public async Task<ApiResponse<DashboardDto>> GetSummaryAsync()
    {
        var balances = await _stockRepo.GetStockBalanceAsync();

        var dto = new DashboardDto
        {
            TotalItems = await _itemRepo.CountAsync(),
            TotalCategories = await _categoryRepo.CountAsync(),
            TotalSuppliers = await _supplierRepo.CountAsync(),
            LowStockItems = balances.Count(b => b.StockStatus == "Low Stock"),
            OutOfStockItems = balances.Count(b => b.StockStatus == "Out of Stock")
        };

        return ApiResponse<DashboardDto>.Ok(dto);
    }
}
