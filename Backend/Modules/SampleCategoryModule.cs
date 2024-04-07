using AutoMapper;
using Backend.DTOs;
using Database.Models;

namespace Backend.Modules;

public class SampleCategoryModule : Module<SampleCategoryDTO, SampleCategory>
{
    private readonly IRelationCheckerModule _relationCheckerModule;
    public SampleCategoryModule(
        IRepository<SampleCategory> repository,
        IMapper mapper,
        IRelationCheckerModule relationCheckerModule) : base(repository, mapper)
    {
        _relationCheckerModule = relationCheckerModule;
    }

    public override async Task<SampleCategoryDTO?> DeleteAsync(string id)
    {
        var sampleCategory = await base.GetById(id);
        if(sampleCategory is not null && _relationCheckerModule.Check(Mapper.Map<SampleCategory>(sampleCategory)) is not null)
        {
            throw new Exception("Can't delete because linked to other data");
        }
        return await base.DeleteAsync(id);
    }
}
