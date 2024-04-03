using AutoMapper;
using Backend.DTOs;
using Database.Models;

namespace Backend.Modules;

public class ReagenModule : Module<ReagenDTO, Reagen>
{
    public ReagenModule(Repository<Reagen> repository, IMapper mapper) : base(repository, mapper)
    {
    }

    public override Task<int> AddAsync(ReagenDTO newItem)
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
