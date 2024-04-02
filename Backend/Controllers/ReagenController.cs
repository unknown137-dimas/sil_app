using System.Collections;
using System.Net;
using Backend.DTOs;
using Backend.Modules.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReagenController : ControllerBase
{
    private readonly ILogger<ReagenController> _logger;
    private readonly IReagenModule _reagenModule;

    public ReagenController(
        ILogger<ReagenController> logger,
        IReagenModule reagenModule
    )
    {
        _logger = logger;
        _reagenModule = reagenModule;
    }


    [HttpGet()]
    public IEnumerable<ReagenDTO> GetAllReagen()
    {
        return _reagenModule.GetAllReagen();
    }

    [HttpGet("{reagenId}")]
    public async Task<ActionResult<ReagenDTO?>> GetReagenByIdAsync(string reagenId)
    {
        var item = await _reagenModule.GetById(reagenId);
        return item is not null ? Ok(item) : NotFound();
    }
}
