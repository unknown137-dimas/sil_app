using Backend.DTOs;
using Backend.Modules;
using Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/reagen")]
public class ReagenController : ControllerBase
{
    private readonly ILogger<ReagenController> _logger;
    private readonly Module<ReagenDTO, Reagen> _reagenModule;

    public ReagenController(
        ILogger<ReagenController> logger,
        Module<ReagenDTO, Reagen> reagenModule
    )
    {
        _logger = logger;
        _reagenModule = reagenModule;
    }


    [HttpGet()]
    public ActionResult<IEnumerable<ReagenDTO>> GetAllReagen()
    {
        var item = _reagenModule.GetAll();
        return item.Equals(Enumerable.Empty<ReagenDTO>()) ? NoContent() : Ok(item);
    }

    [HttpGet("{reagenId}")]
    public async Task<ActionResult<ReagenDTO?>> GetReagenByIdAsync(string reagenId)
    {
        var item = await _reagenModule.GetById(reagenId);
        return item is not null ? Ok(item) : NotFound();
    }

    [HttpPost("create")]
    public async Task<ActionResult<int>> CreateReagen(ReagenDTO newReagen)
    {
        var item = await _reagenModule.AddAsync(newReagen);
        return Ok(item);
    }

    [HttpPost("update")]
    public async Task<ActionResult<int>> UpdateReagen(ReagenDTO updatedReagen)
    {

        var item = await _reagenModule.UpdateAsync(updatedReagen);
        return Ok(item);
    }

    [HttpDelete("delete/{reagenId}")]
    public async Task<ActionResult<int>> DeleteReagen(string reagenId)
    {
        var item = await _reagenModule.DeleteAsync(reagenId);
        return Ok(item);
    }
}
