using MiniInventory.Application.DTOs.Item;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Application.Interfaces.Services;
using MiniInventory.Application.Mapping;
using MiniInventory.Application.Validators;
using MiniInventory.Shared.CommonResponse;

namespace MiniInventory.Application.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _repo;

    public ItemService(IItemRepository repo) => _repo = repo;

    public async Task<ApiResponse<IReadOnlyList<ItemDto>>> GetAllAsync()
    {
        var entities = await _repo.GetAllWithRelationsAsync();
        var data = entities.Select(i => i.ToDto()).ToList();
        return ApiResponse<IReadOnlyList<ItemDto>>.Ok(data);
    }

    public async Task<ApiResponse<ItemDto>> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdWithRelationsAsync(id);
        return entity is null
            ? ApiResponse<ItemDto>.Fail($"Item {id} was not found.")
            : ApiResponse<ItemDto>.Ok(entity.ToDto());
    }

    public async Task<ApiResponse<IReadOnlyList<ItemDto>>> SearchAsync(string keyword)
    {
        var entities = await _repo.SearchAsync(keyword ?? string.Empty);
        var data = entities.Select(i => i.ToDto()).ToList();
        return ApiResponse<IReadOnlyList<ItemDto>>.Ok(data);
    }

    public async Task<ApiResponse<ItemDto>> CreateAsync(ItemCreateDto dto)
    {
        var validation = ItemValidator.Validate(dto);
        if (!validation.IsValid)
            return ApiResponse<ItemDto>.Fail("Validation failed.", validation.Errors);

        if (await _repo.ItemCodeExistsAsync(dto.ItemCode.Trim()))
            return ApiResponse<ItemDto>.Fail($"Item code '{dto.ItemCode}' already exists.");

        var entity = dto.ToEntity();
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();

        var created = await _repo.GetByIdWithRelationsAsync(entity.ItemId);
        return ApiResponse<ItemDto>.Ok(created!.ToDto(), "Item created.");
    }

    public async Task<ApiResponse<ItemDto>> UpdateAsync(int id, ItemUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null)
            return ApiResponse<ItemDto>.Fail($"Item {id} was not found.");

        var validation = ItemValidator.Validate(new ItemCreateDto
        {
            ItemCode = dto.ItemCode,
            Barcode = dto.Barcode,
            ItemName = dto.ItemName,
            CategoryId = dto.CategoryId,
            SupplierId = dto.SupplierId,
            CostPrice = dto.CostPrice,
            SellingPrice = dto.SellingPrice,
            ReorderLevel = dto.ReorderLevel,
            IsActive = dto.IsActive
        });
        if (!validation.IsValid)
            return ApiResponse<ItemDto>.Fail("Validation failed.", validation.Errors);

        if (await _repo.ItemCodeExistsAsync(dto.ItemCode.Trim(), excludeId: id))
            return ApiResponse<ItemDto>.Fail($"Item code '{dto.ItemCode}' already exists.");

        entity.ItemCode = dto.ItemCode.Trim();
        entity.Barcode = dto.Barcode?.Trim();
        entity.ItemName = dto.ItemName.Trim();
        entity.CategoryId = dto.CategoryId;
        entity.SupplierId = dto.SupplierId;
        entity.CostPrice = dto.CostPrice;
        entity.SellingPrice = dto.SellingPrice;
        entity.ReorderLevel = dto.ReorderLevel;
        entity.IsActive = dto.IsActive;

        await _repo.UpdateAsync(entity);
        await _repo.SaveChangesAsync();

        var updated = await _repo.GetByIdWithRelationsAsync(id);
        return ApiResponse<ItemDto>.Ok(updated!.ToDto(), "Item updated.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null)
            return ApiResponse<bool>.Fail($"Item {id} was not found.");

        await _repo.DeleteAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Item deleted.");
    }
}
