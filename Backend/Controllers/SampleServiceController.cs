using Backend.DTOs;
using Backend.Models;
using Backend.Modules;
using Backend.Utilities;
using Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/sample/service")]
public class SampleServiceController : ApiBaseController<SampleServiceController, SampleServiceDTO>
{
    private readonly Module<SampleServiceDTO, SampleService> _sampleServiceModule;

    public SampleServiceController(
        ILogger<SampleServiceController> logger, 
        Module<SampleServiceDTO, SampleService> sampleServiceModule,
        IResponseFactory<SampleServiceDTO> responseFactory
        ) : base(logger, responseFactory)
    {
        _sampleServiceModule = sampleServiceModule;
    }

    [HttpGet()]
    public ActionResult<Response<SampleServiceDTO>> GetAllSampleService()
    {
        var item = _sampleServiceModule.GetAll();
        return GeneratedResponse(item, "");
    }

    [HttpGet("{sampleServiceId}")]
    public async Task<ActionResult<Response<SampleServiceDTO>>> GetSampleServiceByIdAsync(string sampleServiceId)
    {
        var item = await _sampleServiceModule.GetById(sampleServiceId);
        var message = item is not null ? "" : "Item Not Found";
        return GeneratedResponse(item, message);
    }

    [HttpPost()]
    public async Task<ActionResult<Response<SampleServiceDTO>>> CreateSampleService(SampleServiceDTO newSampleService)
    {
        SampleServiceDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _sampleServiceModule.AddAsync(newSampleService);
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

    [HttpPost("{sampleServiceId}")]
    public async Task<ActionResult<Response<SampleServiceDTO>>> UpdateSampleService(string sampleServiceId, SampleServiceDTO updatedSampleService)
    {
        SampleServiceDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _sampleServiceModule.UpdateAsync(sampleServiceId, updatedSampleService);
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

    [HttpDelete("{sampleServiceId}")]
    public async Task<ActionResult<Response<SampleServiceDTO>>> DeleteSampleService(string sampleServiceId)
    {
        SampleServiceDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _sampleServiceModule.DeleteAsync(sampleServiceId);
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
