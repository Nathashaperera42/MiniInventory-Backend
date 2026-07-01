using Microsoft.AspNetCore.Mvc;
using MiniInventory.Application.DTOs.Stock;
using MiniInventory.Application.Interfaces.Services;

namespace MiniInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockService _service;

    public StockController(IStockService service) => _service = service;

    [HttpPost("in")]
    public async Task<IActionResult> StockIn([FromBody] StockInCreateDto dto)
    {
        var result = await _service.StockInAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("out")]
    public async Task<IActionResult> StockOut([FromBody] StockOutCreateDto dto)
    {
        var result = await _service.StockOutAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("in")]
    public async Task<IActionResult> StockInHistory()
        => Ok(await _service.GetStockInHistoryAsync());

    [HttpGet("balance")]
    public async Task<IActionResult> Balance()
        => Ok(await _service.GetBalanceAsync());

    [HttpGet("low-stock")]
    public async Task<IActionResult> LowStock()
        => Ok(await _service.GetLowStockAsync());
}
