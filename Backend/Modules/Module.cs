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


    public virtual async Task<int> AddAsync(DTO newItem)
    {
        using(_repository)
        {
            await _repository.AddAsync(_mapper.Map<Model>(newItem));
            return await _repository.CommitAsync();
        }
    }

    public virtual async Task<int> DeleteAsync(string id)
    {
        using(_repository)
        {
            var itemDeleted = await _repository.GetAsync(id);
            if(itemDeleted is not null)
            {
                _repository.Delete(itemDeleted);
            }
            return await _repository.CommitAsync();
        }
    }

    public virtual IEnumerable<DTO> GetAll()
    {
        using(_repository)
        {
            return _mapper.Map<IEnumerable<DTO>>(_repository.GetEntities());
        }
    }


    public virtual async Task<DTO?> GetById(string id)
    {
        using(_repository)
        {
            return _mapper.Map<DTO>(await _repository.GetAsync(id));
        }
    }

    public virtual async Task<int> UpdateAsync(DTO updatedItem)
    {
        using(_repository)
        {
            _repository.Update(_mapper.Map<Model>(updatedItem));
            return await _repository.CommitAsync();
        }
    }

}
