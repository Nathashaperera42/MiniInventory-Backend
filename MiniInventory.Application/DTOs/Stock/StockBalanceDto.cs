namespace MiniInventory.Application.DTOs.Stock;

public class StockBalanceDto
{
    public int ItemId { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int TotalStockIn { get; set; }
    public int TotalStockOut { get; set; }
    public int CurrentBalance { get; set; }
    public int ReorderLevel { get; set; }
    /// <summary>Good Stock | Low Stock | Out of Stock</summary>
    public string StockStatus { get; set; } = string.Empty;
}
