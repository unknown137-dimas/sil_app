using AutoMapper;
using Backend.DTOs;
using Database.Models;

namespace Backend.Modules;

public class Module<DTO, Model> : IModule<DTO> where DTO : DTOBase where Model : ModelBase
{
    private readonly IRepository<Model> _repository;
    protected readonly IMapper Mapper;

    public Module(
        IRepository<Model> repository,
        IMapper mapper)
    {
        _repository = repository;
        Mapper = mapper;
    }


    public virtual async Task<DTO?> AddAsync(DTO newItem)
    {
        var entityEntry = await _repository.AddAsync(Mapper.Map<Model>(newItem));

        if(await _repository.CommitAsync() > 0)
        {
            return Mapper.Map<DTO>(entityEntry.Entity);
        }
        throw new Exception("Failed To Add Item");
    }

    public virtual async Task<DTO?> DeleteAsync(string id)
    {
        var itemDeleted = await _repository.GetAsync(id);
        if(itemDeleted is null)
        {
            throw new Exception("Item Not Found");
        }

        var entityEntry = _repository.Delete(itemDeleted);

        if(await _repository.CommitAsync() > 0)
        {
            return Mapper.Map<DTO>(entityEntry.Entity);;
        }
        throw new Exception("Failed To Delete Item");
    }

    public virtual IEnumerable<DTO> GetAll()
    {
        return Mapper.Map<IEnumerable<DTO>>(_repository.GetEntities());
    }


    public virtual async Task<DTO?> GetById(string id)
    {
        return Mapper.Map<DTO>(await _repository.GetAsync(id));
    }

    public virtual bool IsExisted(string id)
    {
        return _repository.IsExisted(id);
    }


    public virtual async Task<DTO?> UpdateAsync(DTO updatedItem)
    {
        var existingItem = await _repository.GetAsync(updatedItem.Id!);
        if(existingItem is null)
        {
            throw new Exception("Item Not Found");
        }

        Mapper.Map(updatedItem, existingItem);
        var entityEntry = _repository.Update(existingItem);
        
        if(await _repository.CommitAsync() > 0)
        {
            return Mapper.Map<DTO>(entityEntry.Entity);
        }
        throw new Exception("Failed To Update Item");
    }

}
