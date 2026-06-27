using MiniInventory.Application.DTOs.Category;

namespace MiniInventory.Application.Validators;

public static class CategoryValidator
{
    public static ValidationResultModel Validate(CategoryCreateDto dto)
    {
        var result = new ValidationResultModel();
        result.AddIf(string.IsNullOrWhiteSpace(dto.CategoryName), "Category name is required.");
        result.AddIf(dto.CategoryName?.Length > 100, "Category name must be 100 characters or fewer.");
        return result;
    }
}
