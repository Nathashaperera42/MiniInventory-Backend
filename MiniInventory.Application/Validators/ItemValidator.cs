using MiniInventory.Application.DTOs.Item;

namespace MiniInventory.Application.Validators;

public static class ItemValidator
{
    public static ValidationResultModel Validate(ItemCreateDto dto)
    {
        var result = new ValidationResultModel();
        result.AddIf(string.IsNullOrWhiteSpace(dto.ItemCode), "Item code is required.");
        result.AddIf(string.IsNullOrWhiteSpace(dto.ItemName), "Item name is required.");
        result.AddIf(dto.CategoryId <= 0, "A valid category must be selected.");
        result.AddIf(dto.SupplierId <= 0, "A valid supplier must be selected.");
        result.AddIf(dto.CostPrice < 0, "Cost price cannot be negative.");
        result.AddIf(dto.SellingPrice < 0, "Selling price cannot be negative.");
        result.AddIf(dto.ReorderLevel < 0, "Reorder level cannot be negative.");
        return result;
    }
}
