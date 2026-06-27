using Microsoft.AspNetCore.Mvc;
using MiniInventory.Application.DTOs.Item;
using MiniInventory.Application.Interfaces.Services;

namespace MiniInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    private readonly IItemService _service;

    public ItemController(IItemService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string keyword = "")
        => Ok(await _service.SearchAsync(keyword));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ItemCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ItemUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }
}
