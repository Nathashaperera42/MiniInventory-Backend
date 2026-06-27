using MiniInventory.Application.DTOs.Stock;

namespace MiniInventory.Application.Validators;

public static class StockValidator
{
    public static ValidationResultModel ValidateIn(StockInCreateDto dto)
    {
        var result = new ValidationResultModel();
        result.AddIf(dto.ItemId <= 0, "A valid item must be selected.");
        result.AddIf(dto.SupplierId <= 0, "A valid supplier must be selected.");
        result.AddIf(dto.Quantity <= 0, "Quantity must be greater than zero.");
        result.AddIf(dto.CostPrice < 0, "Cost price cannot be negative.");
        return result;
    }

    public static ValidationResultModel ValidateOut(StockOutCreateDto dto)
    {
        var result = new ValidationResultModel();
        result.AddIf(dto.ItemId <= 0, "A valid item must be selected.");
        result.AddIf(dto.Quantity <= 0, "Quantity must be greater than zero.");
        result.AddIf(dto.Reason < 1 || dto.Reason > 4,
            "Reason must be Sale, Damage, Internal Use or Return.");
        return result;
    }
}
