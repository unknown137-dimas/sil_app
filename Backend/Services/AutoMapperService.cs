using AutoMapper;
using Microsoft.AspNetCore.Identity;

public class AutoMapperService : Profile
{
    public AutoMapperService()
    {
        CreateMap<UserDTO, IdentityUser>().ReverseMap();
    }

    public static IMapper InitializeAutoMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperService>();
        });
        return config.CreateMapper();
    }

}
