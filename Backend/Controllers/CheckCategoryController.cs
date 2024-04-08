using Backend.DTOs;
using Backend.Models;
using Backend.Modules;
using Backend.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/check/category")]
public class CheckCategoryController : ApiBaseController<CheckCategoryController, CheckCategoryDTO>
{
    private readonly CheckCategoryModule _checkCategoryModule;

    public CheckCategoryController(
        ILogger<CheckCategoryController> logger, 
        CheckCategoryModule checkCategoryModule,
        IResponseFactory<CheckCategoryDTO> responseFactory
    ) : base(logger, responseFactory)
    {
        _checkCategoryModule = checkCategoryModule;
    }

    [HttpGet()]
    public ActionResult<Response<CheckCategoryDTO>> GetAllCheckCategory()
    {
        var item = _checkCategoryModule.GetAll();
        return GeneratedResponse(item, "");
    }

    [HttpGet("{checkCategoryId}")]
    public async Task<ActionResult<Response<CheckCategoryDTO>>> GetCheckCategoryByIdAsync(string checkCategoryId)
    {
        var item = await _checkCategoryModule.GetById(checkCategoryId);
        var message = item is not null ? "" : "Item Not Found";
        return GeneratedResponse(item, message);
    }

    [HttpPost()]
    public async Task<ActionResult<Response<CheckCategoryDTO>>> CreateCheckCategory(CheckCategoryDTO newCheckCategory)
    {
        CheckCategoryDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _checkCategoryModule.AddAsync(newCheckCategory);
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

    [HttpPost("{checkCategoryId}")]
    public async Task<ActionResult<Response<CheckCategoryDTO>>> UpdateCheckCategory(string checkCategoryId, CheckCategoryDTO updatedCheckCategory)
    {
        CheckCategoryDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _checkCategoryModule.UpdateAsync(checkCategoryId, updatedCheckCategory);
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

    [HttpDelete("{checkCategoryId}")]
    public async Task<ActionResult<Response<CheckCategoryDTO>>> DeleteCheckCategory(string checkCategoryId)
    {
        CheckCategoryDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _checkCategoryModule.DeleteAsync(checkCategoryId);
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
