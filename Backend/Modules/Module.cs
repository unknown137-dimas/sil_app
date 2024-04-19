using AutoMapper;
using Backend.DTOs;
using Database.Models;

namespace Backend.Modules;

public class Module<DTO, Model> : IModule<DTO> where DTO : DTOBase where Model : ModelBase
{
    protected readonly IRepository<Model> Repository;
    protected readonly IMapper Mapper;

    public Module(
        IRepository<Model> repository,
        IMapper mapper)
    {
        Repository = repository;
        Mapper = mapper;
    }


    public virtual async Task<DTO?> AddAsync(DTO newItem)
    {
        var entityEntry = await Repository.AddAsync(Mapper.Map<Model>(newItem));

        if(await Repository.CommitAsync() > 0)
        {
            return Mapper.Map<DTO>(entityEntry.Entity);
        }
        throw new Exception("Failed To Add Item");
    }

    public virtual async Task<DTO?> DeleteAsync(string id)
    {
        var itemDeleted = await Repository.GetAsync(id);
        if(itemDeleted is null)
        {
            throw new Exception("Item Not Found");
        }

        var entityEntry = Repository.Delete(itemDeleted);

        if(await Repository.CommitAsync() > 0)
        {
            return Mapper.Map<DTO>(entityEntry.Entity);;
        }
        throw new Exception("Failed To Delete Item");
    }

    public virtual IEnumerable<DTO> GetAll()
    {
        return Mapper.Map<IEnumerable<DTO>>(Repository.GetEntities());
    }


    public virtual async Task<DTO?> GetById(string id)
    {
        return Mapper.Map<DTO>(await Repository.GetAsync(id));
    }

    public virtual bool IsExisted(string id)
    {
        return Repository.IsExisted(id);
    }


    public virtual async Task<DTO?> UpdateAsync(string id, DTO updatedItem)
    {
        var existingItem = await Repository.GetAsync(id);
        if(existingItem is null)
        {
            throw new Exception("Item Not Found");
        }

        updatedItem.Id = existingItem.Id;

        Mapper.Map(updatedItem, existingItem);
        var entityEntry = Repository.Update(existingItem);
        
        if(await Repository.CommitAsync() > 0)
        {
            return Mapper.Map<DTO>(entityEntry.Entity);
        }
        throw new Exception("Failed To Update Item");
    }

}
