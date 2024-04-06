using AutoMapper;
using Backend.DTOs;
using Database.Models;

namespace Backend.Modules;

public class MedicalToolModule : Module<MedicalToolDTO, MedicalTool>
{
    public MedicalToolModule(Repository<MedicalTool> repository, IMapper mapper) : base(repository, mapper)
    {
    }

    public override Task<int> AddAsync(MedicalToolDTO newItem)
    {
        var itemExisted = _repository.GetEntities()
            .FirstOrDefault(r => r.Name.Equals(newItem.Name) &&
                r.Code.Equals(newItem.Code)) is not null;
        if(itemExisted)
        {
            return _repository.CommitAsync();
        }
        return base.AddAsync(newItem);
    }
}
