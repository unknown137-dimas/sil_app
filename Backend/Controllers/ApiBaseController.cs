using Backend.Models;
using Backend.Modules;
using Backend.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiBaseController<T, DTO> : ControllerBase where T : ApiBaseController<T, DTO> where DTO : class
{
    protected ILogger<T> Logger { get; }
    private readonly IResponseFactory<DTO> _responseFactory;

    public ApiBaseController(
        ILogger<T> logger,
        IResponseFactory<DTO> responseFactory
        )
    {
      Logger = logger;
      _responseFactory = responseFactory;
    }

    protected ActionResult<Response<DTO>> GeneratedResponse( IEnumerable<DTO> data, string message)
    {
        int statusCode = StatusCodes.Status200OK;
        if(!message.IsNullOrEmpty())
        {
            statusCode = StatusCodes.Status404NotFound;
        }
        return StatusCode(statusCode, _responseFactory.CreateResponse( data, message ));
    }

    protected ActionResult<Response<DTO>> GeneratedResponse( DTO? data, string message)
    {
        int statusCode = StatusCodes.Status200OK;
        if(!message.IsNullOrEmpty())
        {
            statusCode = StatusCodes.Status404NotFound;
        }
        if(data is null)
        {
            statusCode = StatusCodes.Status500InternalServerError;
        }
        return StatusCode(statusCode, _responseFactory.CreateResponse( data, message ));
    }

    protected ActionResult<Response<DTO>> GeneratedResponse( DTO? data, IEnumerable<IdentityError> message)
    {
        int statusCode = StatusCodes.Status200OK;
        if(!message.IsNullOrEmpty())
        {
            statusCode = StatusCodes.Status404NotFound;
        }
        if(data is null)
        {
            statusCode = StatusCodes.Status500InternalServerError;
        }
        return StatusCode(statusCode, _responseFactory.CreateResponse( data, message ));
    }
}
