using MiniInventory.Application.DTOs.Dashboard;
using MiniInventory.Shared.CommonResponse;

namespace MiniInventory.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<ApiResponse<DashboardDto>> GetSummaryAsync();
}
