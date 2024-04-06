using AutoMapper;
using Backend.DTOs;
using Database.Models;

namespace Backend.Modules;

public class Module<DTO, Model> : IModule<DTO> where DTO : DTOBase where Model : ModelBase
{
    protected readonly Repository<Model> _repository;
    protected readonly IMapper _mapper;

    public Module(Repository<Model> repository,
    IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }


    public virtual async Task<DTO?> AddAsync(DTO newItem)
    {
        var entityEntry = await _repository.AddAsync(_mapper.Map<Model>(newItem));

        if(await _repository.CommitAsync() > 0)
        {
            return _mapper.Map<DTO>(entityEntry.Entity);
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
        await _repository.CommitAsync();

        if(await _repository.CommitAsync() > 0)
        {
            return _mapper.Map<DTO>(entityEntry.Entity);;
        }
        throw new Exception("Failed To Delete Item");
    }

    public virtual IEnumerable<DTO> GetAll()
    {
        return _mapper.Map<IEnumerable<DTO>>(_repository.GetEntities());
    }


    public virtual async Task<DTO?> GetById(string id)
    {
        return _mapper.Map<DTO>(await _repository.GetAsync(id));
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

        _mapper.Map(updatedItem, existingItem);
        var entityEntry = _repository.Update(existingItem);
        await _repository.CommitAsync();
        
        if(await _repository.CommitAsync() > 0)
        {
            return _mapper.Map<DTO>(entityEntry.Entity);
        }
        throw new Exception("Failed To Update Item");
    }

}
