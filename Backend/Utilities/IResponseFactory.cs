using Backend.Models;

namespace Backend.Utilities;

public interface IResponseFactory<DTO>
{
    Response<DTO> CreateResponse(DTO? data, string message);
    Response<DTO> CreateResponse(IEnumerable<DTO> data, string message);
}
