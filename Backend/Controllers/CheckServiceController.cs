using Backend.DTOs;
using Backend.Models;
using Backend.Modules;
using Backend.Utilities;
using Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/check/service")]
public class CheckServiceController : ApiBaseController<CheckServiceController, CheckServiceDTO>
{
    private readonly Module<CheckServiceDTO, CheckService> _checkServiceModule;

    public CheckServiceController(
        ILogger<CheckServiceController> logger, 
        Module<CheckServiceDTO, CheckService> checkServiceModule,
        IResponseFactory<CheckServiceDTO> responseFactory
        ) : base(logger, responseFactory)
    {
        _checkServiceModule = checkServiceModule;
    }

    [HttpGet()]
    public ActionResult<Response<CheckServiceDTO>> GetAllCheckService()
    {
        var item = _checkServiceModule.GetAll().OrderBy(cs => cs.Name);
        return GeneratedResponse(item, "");
    }

    [HttpGet("{checkServiceId}")]
    public async Task<ActionResult<Response<CheckServiceDTO>>> GetCheckServiceByIdAsync(string checkServiceId)
    {
        var item = await _checkServiceModule.GetById(checkServiceId);
        var message = item is not null ? "" : "Item Not Found";
        return GeneratedResponse(item, message);
    }

    [HttpPost()]
    public async Task<ActionResult<Response<CheckServiceDTO>>> CreateCheckService(CheckServiceDTO newCheckService)
    {
        CheckServiceDTO? item = null;
        string message = string.Empty;
        try
        {
            newCheckService.Name = textInfo.ToTitleCase(newCheckService.Name.ToLower());
            item = await _checkServiceModule.AddAsync(newCheckService);
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

    [HttpPost("{checkServiceId}")]
    public async Task<ActionResult<Response<CheckServiceDTO>>> UpdateCheckService(string checkServiceId, CheckServiceDTO updatedCheckService)
    {
        CheckServiceDTO? item = null;
        string message = string.Empty;
        try
        {
            updatedCheckService.Name = textInfo.ToTitleCase(updatedCheckService.Name.ToLower());
            item = await _checkServiceModule.UpdateAsync(checkServiceId, updatedCheckService);
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

    [HttpDelete("{checkServiceId}")]
    public async Task<ActionResult<Response<CheckServiceDTO>>> DeleteCheckService(string checkServiceId)
    {
        CheckServiceDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _checkServiceModule.DeleteAsync(checkServiceId);
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

}
