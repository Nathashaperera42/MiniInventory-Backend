namespace MiniInventory.Domain.Entities;

public enum StockOutReason
{
    Sale = 1,
    Damage = 2,
    InternalUse = 3,
    Return = 4
}

public class StockOut
{
    public int StockOutId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public StockOutReason Reason { get; set; }
    public DateTime StockOutDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation
    public Item? Item { get; set; }
}
