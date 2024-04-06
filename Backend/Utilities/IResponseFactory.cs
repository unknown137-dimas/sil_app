using Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace Backend.Utilities;

public interface IResponseFactory<DTO>
{
    Response<DTO> CreateResponse(DTO? data, string message);
    Response<DTO> CreateResponse(IEnumerable<DTO> data, string message);
    Response<DTO> CreateResponse(DTO? data, IEnumerable<IdentityError> message);
    Response<DTO> CreateResponse(IEnumerable<DTO> data, IEnumerable<IdentityError> message);
}
