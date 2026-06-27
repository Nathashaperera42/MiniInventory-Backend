namespace MiniInventory.Application.DTOs.Stock;

public class StockOutDto
{
    public int StockOutId { get; set; }
    public int ItemId { get; set; }
    public string? ItemName { get; set; }
    public int Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime StockOutDate { get; set; }
    public DateTime CreatedDate { get; set; }
}
