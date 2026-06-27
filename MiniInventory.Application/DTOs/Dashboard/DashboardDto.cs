namespace MiniInventory.Application.DTOs.Dashboard;

public class DashboardDto
{
    public int TotalItems { get; set; }
    public int TotalCategories { get; set; }
    public int TotalSuppliers { get; set; }
    public int LowStockItems { get; set; }
    public int OutOfStockItems { get; set; }
}
