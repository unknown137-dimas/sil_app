using AutoMapper;
using Backend.DTOs;
using Database.Models;

namespace Backend.Modules;

public class CheckCategoryModule : Module<CheckCategoryDTO, CheckCategory>
{
    private readonly IRelationCheckerModule _relationCheckerModule;
    public CheckCategoryModule(
        IRepository<CheckCategory> repository,
        IMapper mapper,
        IRelationCheckerModule relationCheckerModule) : base(repository, mapper)
    {
        _relationCheckerModule = relationCheckerModule;
    }

    public override async Task<CheckCategoryDTO?> DeleteAsync(string id)
    {
        var checkCategory = await base.GetById(id);
        if(checkCategory is not null && _relationCheckerModule.Check(Mapper.Map<CheckCategory>(checkCategory)) is not null)
        {
            throw new Exception("Can't delete because linked to other data");
        }
        return await base.DeleteAsync(id);
    }
}
