using Microsoft.Extensions.DependencyInjection;
using MiniInventory.Application.Interfaces.Services;
using MiniInventory.Application.Services;

namespace MiniInventory.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<IDashboardService, DashboardService>();
        return services;
    }
}
