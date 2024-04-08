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
        CreateMap<RoleAuthActionDTO, RoleAuthAction>().ReverseMap();
        CreateMap<PatientDTO, Patient>().ReverseMap();
        CreateMap<PatientCheckDTO, PatientCheck>().ReverseMap();
        CreateMap<PatientCheckResultDTO, PatientCheckResult>().ReverseMap();
        CreateMap<PatientSampleDTO, PatientSample>().ReverseMap();
        CreateMap<PatientSampleResultDTO, PatientSampleResult>().ReverseMap();
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
