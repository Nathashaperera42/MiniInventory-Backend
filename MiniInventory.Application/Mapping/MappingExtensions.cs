using MiniInventory.Application.DTOs.Category;
using MiniInventory.Application.DTOs.Item;
using MiniInventory.Application.DTOs.Stock;
using MiniInventory.Application.DTOs.Supplier;
using MiniInventory.Domain.Entities;

namespace MiniInventory.Application.Mapping;

/// <summary>
/// Lightweight hand-written mappers between entities and DTOs.
/// Keeps the project dependency-free and easy for interns to follow.
/// </summary>
public static class MappingExtensions
{
    // ----- Category -----
    public static CategoryDto ToDto(this Category c) => new()
    {
        CategoryId = c.CategoryId,
        CategoryName = c.CategoryName,
        Description = c.Description,
        IsActive = c.IsActive,
        CreatedDate = c.CreatedDate
    };

    public static Category ToEntity(this CategoryCreateDto dto) => new()
    {
        CategoryName = dto.CategoryName.Trim(),
        Description = dto.Description?.Trim(),
        IsActive = dto.IsActive,
        CreatedDate = DateTime.UtcNow
    };

    // ----- Supplier -----
    public static SupplierDto ToDto(this Supplier s) => new()
    {
        SupplierId = s.SupplierId,
        SupplierName = s.SupplierName,
        ContactNumber = s.ContactNumber,
        Email = s.Email,
        Address = s.Address,
        IsActive = s.IsActive,
        CreatedDate = s.CreatedDate
    };

    public static Supplier ToEntity(this SupplierCreateDto dto) => new()
    {
        SupplierName = dto.SupplierName.Trim(),
        ContactNumber = dto.ContactNumber?.Trim(),
        Email = dto.Email?.Trim(),
        Address = dto.Address?.Trim(),
        IsActive = dto.IsActive,
        CreatedDate = DateTime.UtcNow
    };

    // ----- Item -----
    public static ItemDto ToDto(this Item i) => new()
    {
        ItemId = i.ItemId,
        ItemCode = i.ItemCode,
        Barcode = i.Barcode,
        ItemName = i.ItemName,
        CategoryId = i.CategoryId,
        CategoryName = i.Category?.CategoryName,
        SupplierId = i.SupplierId,
        SupplierName = i.Supplier?.SupplierName,
        CostPrice = i.CostPrice,
        SellingPrice = i.SellingPrice,
        ReorderLevel = i.ReorderLevel,
        IsActive = i.IsActive,
        CreatedDate = i.CreatedDate
    };

    public static Item ToEntity(this ItemCreateDto dto) => new()
    {
        ItemCode = dto.ItemCode.Trim(),
        Barcode = dto.Barcode?.Trim(),
        ItemName = dto.ItemName.Trim(),
        CategoryId = dto.CategoryId,
        SupplierId = dto.SupplierId,
        CostPrice = dto.CostPrice,
        SellingPrice = dto.SellingPrice,
        ReorderLevel = dto.ReorderLevel,
        IsActive = dto.IsActive,
        CreatedDate = DateTime.UtcNow
    };

    // ----- Stock -----
    public static StockInDto ToDto(this StockIn s) => new()
    {
        StockInId = s.StockInId,
        ItemId = s.ItemId,
        ItemName = s.Item?.ItemName,
        SupplierId = s.SupplierId,
        SupplierName = s.Supplier?.SupplierName,
        Quantity = s.Quantity,
        CostPrice = s.CostPrice,
        StockInDate = s.StockInDate,
        CreatedDate = s.CreatedDate
    };

    public static StockOutDto ToDto(this StockOut s) => new()
    {
        StockOutId = s.StockOutId,
        ItemId = s.ItemId,
        ItemName = s.Item?.ItemName,
        Quantity = s.Quantity,
        Reason = s.Reason.ToString(),
        StockOutDate = s.StockOutDate,
        CreatedDate = s.CreatedDate
    };
}
