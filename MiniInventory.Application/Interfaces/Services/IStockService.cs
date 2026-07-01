using MiniInventory.Application.DTOs.Stock;
using MiniInventory.Shared.CommonResponse;

namespace MiniInventory.Application.Interfaces.Services;

public interface IStockService
{
    Task<ApiResponse<StockInDto>> StockInAsync(StockInCreateDto dto);
    Task<ApiResponse<StockOutDto>> StockOutAsync(StockOutCreateDto dto);
    Task<ApiResponse<IReadOnlyList<StockBalanceDto>>> GetBalanceAsync();
    Task<ApiResponse<IReadOnlyList<StockBalanceDto>>> GetLowStockAsync();
    Task<ApiResponse<IReadOnlyList<StockInDto>>> GetStockInHistoryAsync();
    Task<ApiResponse<IReadOnlyList<StockOutDto>>> GetStockOutHistoryAsync();
}
