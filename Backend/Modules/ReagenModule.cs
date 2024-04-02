using AutoMapper;
using Backend.DTOs;
using Backend.Modules.Interfaces;
using Database.Models;

namespace Backend.Modules;

public class ReagenModule : IReagenModule
{
    protected readonly Repository<Reagen> _reagenRepository;
    protected readonly IMapper _mapper;

    public ReagenModule(Repository<Reagen> reagenRepository,
    IMapper mapper)
    {
        _reagenRepository = reagenRepository;
        _mapper = mapper;
    }


    public async Task<int> AddReagenAsync(ReagenDTO newReagen)
    {
        using(_reagenRepository)
        {
            await _reagenRepository.AddAsync(_mapper.Map<Reagen>(newReagen));
            return await _reagenRepository.CommitAsync();
        }
    }

    public async Task<int> DeleteReagenAsync(string reagenId)
    {
        using(_reagenRepository)
        {
            var itemDeleted = await _reagenRepository.GetAsync(reagenId);
            if(itemDeleted is not null)
            {
                _reagenRepository.Delete(itemDeleted);
            }
            return await _reagenRepository.CommitAsync();
        }
    }

    public async Task<ReagenDTO?> GetById(string reagenId)
    {
        using(_reagenRepository)
        {
            return _mapper.Map<ReagenDTO>(await _reagenRepository.GetAsync(reagenId));
        }
    }

    public async Task<int> UpdateReagenAsync(ReagenDTO updatedReagen)
    {
        using(_reagenRepository)
        {
            _reagenRepository.Update(_mapper.Map<Reagen>(updatedReagen));
            return await _reagenRepository.CommitAsync();
        }
    }

}
