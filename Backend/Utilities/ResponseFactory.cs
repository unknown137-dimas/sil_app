using Backend.Models;

namespace Backend.Utilities;

public class ResponseFactory<DTO> : IResponseFactory<DTO>
{
    public Response<DTO> CreateResponse(DTO? data, string message)
    {
        return new(){
            Data = data is null ? [] : [data],
            Messages = [message]
        };
    }

    public Response<DTO> CreateResponse(IEnumerable<DTO> data, string message)
    {
        return new(){
            Data = data,
            Messages = [message]
        };
    }

    public Response<DTO> CreateResponse(DTO? data, IEnumerable<object> message)
    {
        return new(){
            Data = data is null ? [] : [data],
            Messages = message
        };
    }

    public Response<DTO> CreateResponse(IEnumerable<DTO> data, IEnumerable<object> message)
    {
        return new(){
            Data = data,
            Messages = message
        };
    }

}
