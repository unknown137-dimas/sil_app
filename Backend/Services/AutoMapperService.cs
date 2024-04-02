using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Backend.Services;

public class AutoMapperService : Profile
{
    public AutoMapperService()
    {
        CreateMap<UserDTO, User>().ReverseMap();
        CreateMap<CheckCategoryDTO, CheckCategory>().ReverseMap();
        CreateMap<CheckServiceDTO, CheckService>().ReverseMap();
        CreateMap<MedicalToolDTO, MedicalTool>().ReverseMap();
        CreateMap<RoleDTO, IdentityRole>().ReverseMap();
        CreateMap<SampleCategoryDTO, SampleCategory>().ReverseMap();
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
