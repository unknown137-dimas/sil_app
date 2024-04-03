using Backend.DTOs;
using Backend.Modules;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/reagen")]
public class ReagenController : ControllerBase
{
    private readonly ILogger<ReagenController> _logger;
    private readonly ReagenModule _reagenModule;

    public ReagenController(
        ILogger<ReagenController> logger,
        ReagenModule reagenModule
    )
    {
        _logger = logger;
        _reagenModule = reagenModule;
    }


    [HttpGet()]
    public ActionResult<IEnumerable<ReagenDTO>> GetAllReagen()
    {
        var item = _reagenModule.GetAll();
        return Ok(item);
    }

    [HttpGet("{reagenId}")]
    public async Task<ActionResult<ReagenDTO?>> GetReagenByIdAsync(string reagenId)
    {
        var item = await _reagenModule.GetById(reagenId);
        return item is not null ? Ok(item) : NotFound();
    }

    [HttpPost()]
    public async Task<ActionResult<int>> CreateReagen(ReagenDTO newReagen)
    {
        var item = await _reagenModule.AddAsync(newReagen);
        return item > 0 ? Ok(item) : BadRequest();
    }

    [HttpPost("{reagenId}")]
    public async Task<ActionResult<ReagenDTO>> UpdateReagen(string reagenId, ReagenDTO updatedReagen)
    {
        int item = 0;
        if(_reagenModule.IsExisted(reagenId))
        {
            item = await _reagenModule.UpdateAsync(updatedReagen);
        }
        return item > 0 ? Ok(updatedReagen) : BadRequest("Failed to update");
    }

    [HttpDelete("{reagenId}")]
    public async Task<ActionResult<int>> DeleteReagen(string reagenId)
    {
        var item = await _reagenModule.DeleteAsync(reagenId);
        return item > 0 ? Ok(item) : BadRequest();
    }
}
