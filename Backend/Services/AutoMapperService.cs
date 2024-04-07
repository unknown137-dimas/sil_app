using AutoMapper;
using Backend.DTOs;
using Database.Models;
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
        CreateMap<ReagenDTO, Reagen>().ReverseMap();
        CreateMap<RoleDTO, IdentityRole>().ReverseMap();
        CreateMap<SampleCategoryDTO, SampleCategory>().ReverseMap();
        CreateMap<SampleServiceDTO, SampleService>().ReverseMap();
        CreateMap<AuthActionDTO, AuthAction>().ReverseMap();
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
