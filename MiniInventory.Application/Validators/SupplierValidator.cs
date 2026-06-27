using MiniInventory.Application.DTOs.Supplier;

namespace MiniInventory.Application.Validators;

public static class SupplierValidator
{
    public static ValidationResultModel Validate(SupplierCreateDto dto)
    {
        var result = new ValidationResultModel();
        result.AddIf(string.IsNullOrWhiteSpace(dto.SupplierName), "Supplier name is required.");
        result.AddIf(!string.IsNullOrWhiteSpace(dto.Email) && !dto.Email.Contains('@'),
            "Email format is invalid.");
        return result;
    }
}
