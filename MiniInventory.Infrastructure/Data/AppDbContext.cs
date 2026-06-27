using Microsoft.EntityFrameworkCore;
using MiniInventory.Domain.Entities;

namespace MiniInventory.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<StockIn> StockIns => Set<StockIn>();
    public DbSet<StockOut> StockOuts => Set<StockOut>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ----- Category -----
        modelBuilder.Entity<Category>(e =>
        {
            e.ToTable("CategoryTable");
            e.HasKey(x => x.CategoryId);
            e.Property(x => x.CategoryName).IsRequired().HasMaxLength(100);
            e.Property(x => x.Description).HasMaxLength(250);
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            e.HasIndex(x => x.CategoryName);
        });

        // ----- Supplier -----
        modelBuilder.Entity<Supplier>(e =>
        {
            e.ToTable("SupplierTable");
            e.HasKey(x => x.SupplierId);
            e.Property(x => x.SupplierName).IsRequired().HasMaxLength(150);
            e.Property(x => x.ContactNumber).HasMaxLength(20);
            e.Property(x => x.Email).HasMaxLength(150);
            e.Property(x => x.Address).HasMaxLength(300);
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
        });

        // ----- Item -----
        modelBuilder.Entity<Item>(e =>
        {
            e.ToTable("ItemTable");
            e.HasKey(x => x.ItemId);
            e.Property(x => x.ItemCode).IsRequired().HasMaxLength(50);
            e.Property(x => x.Barcode).HasMaxLength(50);
            e.Property(x => x.ItemName).IsRequired().HasMaxLength(200);
            e.Property(x => x.CostPrice).HasColumnType("decimal(18,2)");
            e.Property(x => x.SellingPrice).HasColumnType("decimal(18,2)");
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETUTCDATE()");

            e.HasIndex(x => x.ItemCode).IsUnique();
            e.HasIndex(x => x.Barcode);
            e.HasIndex(x => x.ItemName);
            e.HasIndex(x => x.CategoryId);
            e.HasIndex(x => x.SupplierId);

            e.HasOne(x => x.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Supplier)
                .WithMany(s => s.Items)
                .HasForeignKey(x => x.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ----- StockIn -----
        modelBuilder.Entity<StockIn>(e =>
        {
            e.ToTable("StockInTable");
            e.HasKey(x => x.StockInId);
            e.Property(x => x.CostPrice).HasColumnType("decimal(18,2)");
            e.Property(x => x.StockInDate).HasDefaultValueSql("GETUTCDATE()");
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            e.HasIndex(x => x.ItemId);

            e.HasOne(x => x.Item)
                .WithMany(i => i.StockIns)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Supplier)
                .WithMany(s => s.StockIns)
                .HasForeignKey(x => x.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ----- StockOut -----
        modelBuilder.Entity<StockOut>(e =>
        {
            e.ToTable("StockOutTable");
            e.HasKey(x => x.StockOutId);
            e.Property(x => x.Reason).HasConversion<int>();
            e.Property(x => x.StockOutDate).HasDefaultValueSql("GETUTCDATE()");
            e.Property(x => x.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            e.HasIndex(x => x.ItemId);

            e.HasOne(x => x.Item)
                .WithMany(i => i.StockOuts)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
