using Backend.DTOs;
using Backend.Models;
using Backend.Modules;
using Backend.Utilities;
using Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/reagen")]
public class ReagenController : ApiBaseController<ReagenController, ReagenDTO>
{
    private readonly Module<ReagenDTO, Reagen> _reagenModule;

    public ReagenController(
        ILogger<ReagenController> logger,
        Module<ReagenDTO, Reagen> reagenModule,
        IResponseFactory<ReagenDTO> responseFactory
    ) : base(logger, responseFactory)
    {
        _reagenModule = reagenModule;
    }


    [HttpGet()]
    public ActionResult<Response<ReagenDTO>> GetAllReagen()
    {
        var item = _reagenModule.GetAll();
        return GeneratedResponse(item, "");
    }

    [HttpGet("{reagenId}")]
    public async Task<ActionResult<Response<ReagenDTO>>> GetReagenByIdAsync(string reagenId)
    {
        var item = await _reagenModule.GetById(reagenId);
        var message = item is not null ? "" : "Item Not Found";
        return GeneratedResponse(item, message);
    }

    [HttpPost()]
    public async Task<ActionResult<Response<ReagenDTO>>> CreateReagen(ReagenDTO newReagen)
    {
        ReagenDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _reagenModule.AddAsync(newReagen);
        }
        catch (Exception ex)
        {
            message = ex.Message;
            if(ex.InnerException is not null)
            {
                message = ex.InnerException.Message;
            }
        }
        return GeneratedResponse(item, message);
    }

    [HttpPost("{reagenId}")]
    public async Task<ActionResult<Response<ReagenDTO>>> UpdateReagen(ReagenDTO updatedReagen)
    {
        ReagenDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _reagenModule.UpdateAsync(updatedReagen);
        }
        catch (Exception ex)
        {
            message = ex.Message;
        }
        return GeneratedResponse(item, message);
    }

    [HttpDelete("{reagenId}")]
    public async Task<ActionResult<Response<ReagenDTO>>> DeleteReagen(string reagenId)
    {
        ReagenDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _reagenModule.DeleteAsync(reagenId);
        }
        catch (Exception ex)
        {
            message = ex.Message;
        }
        return GeneratedResponse(item, message);
    }
}
