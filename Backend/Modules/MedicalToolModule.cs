using AutoMapper;
using Backend.DTOs;
using Backend.Modules.Interfaces;
using Database.Models;

namespace Backend.Modules;

public class MedicalToolModule : IMedicalToolModule
{
    protected readonly Repository<MedicalTool> _medicalToolRepository;
    protected readonly IMapper _mapper;

    public MedicalToolModule(Repository<MedicalTool> medicalToolRepository,
    IMapper mapper)
    {
        _medicalToolRepository = medicalToolRepository;
        _mapper = mapper;
    }


    public async Task<int> AddMedicalToolAsync(MedicalToolDTO newMedicalTool)
    {
        using(_medicalToolRepository)
        {
            await _medicalToolRepository.AddAsync(_mapper.Map<MedicalTool>(newMedicalTool));
            return await _medicalToolRepository.CommitAsync();
        }
    }

    public async Task<int> DeleteMedicalToolAsync(string medicalToolId)
    {
        using(_medicalToolRepository)
        {
            var itemDeleted = await _medicalToolRepository.GetAsync(medicalToolId);
            if(itemDeleted is not null)
            {
                _medicalToolRepository.Delete(itemDeleted);
            }
            return await _medicalToolRepository.CommitAsync();
        }
    }

    public async Task<MedicalToolDTO?> GetById(string medicalToolId)
    {
        using(_medicalToolRepository)
        {
            return _mapper.Map<MedicalToolDTO>(await _medicalToolRepository.GetAsync(medicalToolId));
        }
    }

    public async Task<int> UpdateMedicalToolAsync(MedicalToolDTO updatedMedicalTool)
    {
        using(_medicalToolRepository)
        {
            _medicalToolRepository.Update(_mapper.Map<MedicalTool>(updatedMedicalTool));
            return await _medicalToolRepository.CommitAsync();
        }
    }

}
