using Backend.DTOs;
using Backend.Models;
using Backend.Modules;
using Backend.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/sample/category")]
public class SampleCategoryController : ApiBaseController<SampleCategoryController, SampleCategoryDTO>
{
    private readonly SampleCategoryModule _sampleCategoryModule;

    public SampleCategoryController(
        ILogger<SampleCategoryController> logger, 
        SampleCategoryModule checkCategoryModule,
        IResponseFactory<SampleCategoryDTO> responseFactory
    ) : base(logger, responseFactory)
    {
        _sampleCategoryModule = checkCategoryModule;
    }

    [HttpGet()]
    public ActionResult<Response<SampleCategoryDTO>> GetAllSampleCategory()
    {
        var item = _sampleCategoryModule.GetAll();
        return GeneratedResponse(item, "");
    }

    [HttpGet("{sampleCategoryId}")]
    public async Task<ActionResult<Response<SampleCategoryDTO>>> GetSampleCategoryByIdAsync(string sampleCategoryId)
    {
        var item = await _sampleCategoryModule.GetById(sampleCategoryId);
        var message = item is not null ? "" : "Item Not Found";
        return GeneratedResponse(item, message);
    }

    [HttpPost()]
    public async Task<ActionResult<Response<SampleCategoryDTO>>> CreateSampleCategory(SampleCategoryDTO newSampleCategory)
    {
        SampleCategoryDTO? item = null;
        string message = string.Empty;
        try
        {
            newSampleCategory.Name = textInfo.ToTitleCase(newSampleCategory.Name.ToLower());
            item = await _sampleCategoryModule.AddAsync(newSampleCategory);
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

    [HttpPost("{sampleCategoryId}")]
    public async Task<ActionResult<Response<SampleCategoryDTO>>> UpdateSampleCategory(string sampleCategoryId, SampleCategoryDTO updatedSampleCategory)
    {
        SampleCategoryDTO? item = null;
        string message = string.Empty;
        try
        {
            updatedSampleCategory.Name = textInfo.ToTitleCase(updatedSampleCategory.Name.ToLower());
            item = await _sampleCategoryModule.UpdateAsync(sampleCategoryId, updatedSampleCategory);
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

    [HttpDelete("{sampleCategoryId}")]
    public async Task<ActionResult<Response<SampleCategoryDTO>>> DeleteSampleCategory(string sampleCategoryId)
    {
        SampleCategoryDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _sampleCategoryModule.DeleteAsync(sampleCategoryId);
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
