using Backend.DTOs;
using Backend.Models;
using Backend.Modules;
using Backend.Utilities;
using Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/auth/action")]
public class AuthActionController : ApiBaseController<AuthActionController, AuthActionDTO>
{
    private readonly Module<AuthActionDTO, AuthAction> _authActionModule;

    public AuthActionController(
        ILogger<AuthActionController> logger, 
        Module<AuthActionDTO, AuthAction> authActionModule,
        IResponseFactory<AuthActionDTO> responseFactory
    ) : base(logger, responseFactory)
    {
        _authActionModule = authActionModule;
    }

    [HttpGet()]
    public ActionResult<Response<AuthActionDTO>> GetAllAuthAction()
    {
        var item = _authActionModule.GetAll();
        return GeneratedResponse(item, "");
    }

    [HttpGet("{authActionId}")]
    public async Task<ActionResult<Response<AuthActionDTO>>> GetAuthActionByIdAsync(string authActionId)
    {
        var item = await _authActionModule.GetById(authActionId);
        var message = item is not null ? "" : "Item Not Found";
        return GeneratedResponse(item, message);
    }

    [HttpPost()]
    public async Task<ActionResult<Response<AuthActionDTO>>> CreateAuthAction(AuthActionDTO newAuthAction)
    {
        AuthActionDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _authActionModule.AddAsync(newAuthAction);
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

    [HttpPost("{authActionId}")]
    public async Task<ActionResult<Response<AuthActionDTO>>> UpdateAuthAction(string authActionId, AuthActionDTO updatedAuthAction)
    {
        AuthActionDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _authActionModule.UpdateAsync(authActionId, updatedAuthAction);
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

    [HttpDelete("{authActionId}")]
    public async Task<ActionResult<Response<AuthActionDTO>>> DeleteAuthAction(string authActionId)
    {
        AuthActionDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _authActionModule.DeleteAsync(authActionId);
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
