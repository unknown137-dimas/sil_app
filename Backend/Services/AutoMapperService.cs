using AutoMapper;

public class AutoMapperService : Profile
{
    public AutoMapperService()
    {
        CreateMap<UserDTO, User>().ReverseMap();
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
