using MiniInventory.Application.DTOs.Stock;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Application.Interfaces.Services;
using MiniInventory.Application.Mapping;
using MiniInventory.Application.Validators;
using MiniInventory.Domain.Entities;
using MiniInventory.Shared.CommonResponse;

namespace MiniInventory.Application.Services;

public class StockService : IStockService
{
    private readonly IStockRepository _stockRepo;
    private readonly IItemRepository _itemRepo;

    public StockService(IStockRepository stockRepo, IItemRepository itemRepo)
    {
        _stockRepo = stockRepo;
        _itemRepo = itemRepo;
    }

    public async Task<ApiResponse<StockInDto>> StockInAsync(StockInCreateDto dto)
    {
        var validation = StockValidator.ValidateIn(dto);
        if (!validation.IsValid)
            return ApiResponse<StockInDto>.Fail("Validation failed.", validation.Errors);

        if (await _itemRepo.GetByIdAsync(dto.ItemId) is null)
            return ApiResponse<StockInDto>.Fail($"Item {dto.ItemId} was not found.");

        var entity = new StockIn
        {
            ItemId = dto.ItemId,
            SupplierId = dto.SupplierId,
            Quantity = dto.Quantity,
            CostPrice = dto.CostPrice,
            StockInDate = dto.StockInDate ?? DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow
        };

        await _stockRepo.AddStockInAsync(entity);
        await _stockRepo.SaveChangesAsync();
        return ApiResponse<StockInDto>.Ok(entity.ToDto(), "Stock in recorded.");
    }

    public async Task<ApiResponse<StockOutDto>> StockOutAsync(StockOutCreateDto dto)
    {
        var validation = StockValidator.ValidateOut(dto);
        if (!validation.IsValid)
            return ApiResponse<StockOutDto>.Fail("Validation failed.", validation.Errors);

        if (await _itemRepo.GetByIdAsync(dto.ItemId) is null)
            return ApiResponse<StockOutDto>.Fail($"Item {dto.ItemId} was not found.");

        // Business rule: cannot remove more stock than is currently available.
        var balance = await _stockRepo.CurrentBalanceAsync(dto.ItemId);
        if (dto.Quantity > balance)
            return ApiResponse<StockOutDto>.Fail(
                $"Insufficient stock. Available balance is {balance}.");

        var entity = new StockOut
        {
            ItemId = dto.ItemId,
            Quantity = dto.Quantity,
            Reason = (StockOutReason)dto.Reason,
            StockOutDate = dto.StockOutDate ?? DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow
        };

        await _stockRepo.AddStockOutAsync(entity);
        await _stockRepo.SaveChangesAsync();
        return ApiResponse<StockOutDto>.Ok(entity.ToDto(), "Stock out recorded.");
    }

    public async Task<ApiResponse<IReadOnlyList<StockBalanceDto>>> GetBalanceAsync()
    {
        var data = await _stockRepo.GetStockBalanceAsync();
        return ApiResponse<IReadOnlyList<StockBalanceDto>>.Ok(data);
    }

    public async Task<ApiResponse<IReadOnlyList<StockBalanceDto>>> GetLowStockAsync()
    {
        var data = await _stockRepo.GetLowStockAsync();
        return ApiResponse<IReadOnlyList<StockBalanceDto>>.Ok(data);
    }
}
