using MiniInventory.Application.DTOs.Category;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Application.Interfaces.Services;
using MiniInventory.Application.Mapping;
using MiniInventory.Application.Validators;
using MiniInventory.Shared.CommonResponse;

namespace MiniInventory.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repo;

    public CategoryService(ICategoryRepository repo) => _repo = repo;

    public async Task<ApiResponse<IReadOnlyList<CategoryDto>>> GetAllAsync()
    {
        var entities = await _repo.GetAllAsync();
        var data = entities.Select(c => c.ToDto()).ToList();
        return ApiResponse<IReadOnlyList<CategoryDto>>.Ok(data);
    }

    public async Task<ApiResponse<CategoryDto>> GetByIdAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null
            ? ApiResponse<CategoryDto>.Fail($"Category {id} was not found.")
            : ApiResponse<CategoryDto>.Ok(entity.ToDto());
    }

    public async Task<ApiResponse<IReadOnlyList<CategoryDto>>> SearchAsync(string keyword)
    {
        var entities = await _repo.SearchAsync(keyword ?? string.Empty);
        var data = entities.Select(c => c.ToDto()).ToList();
        return ApiResponse<IReadOnlyList<CategoryDto>>.Ok(data);
    }

    public async Task<ApiResponse<CategoryDto>> CreateAsync(CategoryCreateDto dto)
    {
        var validation = CategoryValidator.Validate(dto);
        if (!validation.IsValid)
            return ApiResponse<CategoryDto>.Fail("Validation failed.", validation.Errors);

        var entity = dto.ToEntity();
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<CategoryDto>.Ok(entity.ToDto(), "Category created.");
    }

    public async Task<ApiResponse<CategoryDto>> UpdateAsync(int id, CategoryUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null)
            return ApiResponse<CategoryDto>.Fail($"Category {id} was not found.");

        var validation = CategoryValidator.Validate(new CategoryCreateDto
        {
            CategoryName = dto.CategoryName,
            Description = dto.Description,
            IsActive = dto.IsActive
        });
        if (!validation.IsValid)
            return ApiResponse<CategoryDto>.Fail("Validation failed.", validation.Errors);

        entity.CategoryName = dto.CategoryName.Trim();
        entity.Description = dto.Description?.Trim();
        entity.IsActive = dto.IsActive;

        await _repo.UpdateAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<CategoryDto>.Ok(entity.ToDto(), "Category updated.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null)
            return ApiResponse<bool>.Fail($"Category {id} was not found.");

        await _repo.DeleteAsync(entity);
        await _repo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Category deleted.");
    }
}
