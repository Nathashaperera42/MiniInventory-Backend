using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniInventory.Application.Interfaces.Repositories;
using MiniInventory.Infrastructure.Data;
using MiniInventory.Infrastructure.Repositories;

namespace MiniInventory.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IStockRepository, StockRepository>();

        return services;
    }
}
