using MiniInventory.Application.DTOs.Category;
using MiniInventory.Shared.CommonResponse;

namespace MiniInventory.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<ApiResponse<IReadOnlyList<CategoryDto>>> GetAllAsync();
    Task<ApiResponse<CategoryDto>> GetByIdAsync(int id);
    Task<ApiResponse<IReadOnlyList<CategoryDto>>> SearchAsync(string keyword);
    Task<ApiResponse<CategoryDto>> CreateAsync(CategoryCreateDto dto);
    Task<ApiResponse<CategoryDto>> UpdateAsync(int id, CategoryUpdateDto dto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
}
