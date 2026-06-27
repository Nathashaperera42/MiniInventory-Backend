namespace MiniInventory.Application.DTOs.Supplier;

public class SupplierCreateDto
{
    public string SupplierName { get; set; } = string.Empty;
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
}
