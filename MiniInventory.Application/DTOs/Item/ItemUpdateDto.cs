namespace MiniInventory.Application.DTOs.Item;

public class ItemUpdateDto
{
    public string ItemCode { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int SupplierId { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public int ReorderLevel { get; set; }
    public bool IsActive { get; set; } = true;
}
