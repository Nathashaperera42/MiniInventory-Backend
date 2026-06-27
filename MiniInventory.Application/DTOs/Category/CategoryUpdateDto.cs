namespace MiniInventory.Application.DTOs.Category;

public class CategoryUpdateDto
{
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
