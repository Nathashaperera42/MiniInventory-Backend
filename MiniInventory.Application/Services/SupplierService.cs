using MiniInventory.Application.DTOs.Supplier;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Application.Interfaces.Services;
using MiniInventory.Application.Mapping;
using MiniInventory.Application.Validators;
using MiniInventory.Shared.CommonResponse;

namespace MiniInventory.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repo;

    public SupplierService(ISupplierRepository repo) => _repo = repo;

    public async Task<ApiResponse<IReadOnlyList<SupplierDto>>> GetAllAsync()
    {
        var entities = await _repo.GetAllAsync();
        var data = entities.Select(s => s.ToDto()).ToList();
        return ApiResponse<IReadOnlyList<SupplierDto>>.Ok(data);
    }

    public async Task<ApiResponse<SupplierDto>> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null
            ? ApiResponse<SupplierDto>.Fail($"Supplier {id} was not found.")
            : ApiResponse<SupplierDto>.Ok(entity.ToDto());
    }

    public async Task<ApiResponse<SupplierDto>> CreateAsync(SupplierCreateDto dto)
    {
        var validation = SupplierValidator.Validate(dto);
        if (!validation.IsValid)
            return ApiResponse<SupplierDto>.Fail("Validation failed.", validation.Errors);

        var entity = dto.ToEntity();
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<SupplierDto>.Ok(entity.ToDto(), "Supplier created.");
    }

    public async Task<ApiResponse<SupplierDto>> UpdateAsync(int id, SupplierUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null)
            return ApiResponse<SupplierDto>.Fail($"Supplier {id} was not found.");

        entity.SupplierName = dto.SupplierName.Trim();
        entity.ContactNumber = dto.ContactNumber?.Trim();
        entity.Email = dto.Email?.Trim();
        entity.Address = dto.Address?.Trim();
        entity.IsActive = dto.IsActive;

        await _repo.UpdateAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<SupplierDto>.Ok(entity.ToDto(), "Supplier updated.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null)
            return ApiResponse<bool>.Fail($"Supplier {id} was not found.");

        await _repo.DeleteAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Supplier deleted.");
    }
}
