using Microsoft.AspNetCore.Mvc;
using MiniInventory.Application.Interfaces.Services;

namespace MiniInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service) => _service = service;

    [HttpGet("summary")]
    public async Task<IActionResult> Summary()
        => Ok(await _service.GetSummaryAsync());
}
