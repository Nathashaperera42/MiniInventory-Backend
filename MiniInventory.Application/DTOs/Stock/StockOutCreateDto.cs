namespace MiniInventory.Application.DTOs.Stock;

public class StockOutCreateDto
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    /// <summary>Sale = 1, Damage = 2, InternalUse = 3, Return = 4</summary>
    public int Reason { get; set; }
    public DateTime? StockOutDate { get; set; }
}
