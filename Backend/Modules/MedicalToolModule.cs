using AutoMapper;
using Backend.DTOs;
using Database.Models;

namespace Backend.Modules;

public class MedicalToolModule : Module<MedicalToolDTO, MedicalTool>
{
    
    public MedicalToolModule(IRepository<MedicalTool> repository, IMapper mapper) : base(repository, mapper)
    {
    }

    public override async Task<MedicalToolDTO?> AddAsync(MedicalToolDTO newItem)
    {
        newItem.LastCalibrationDate = DateTime.Now;
        newItem.CalibrationDate = DateTime.Now;
        return await base.AddAsync(newItem);
    }

    public override async Task<MedicalToolDTO?> UpdateAsync(string id, MedicalToolDTO updatedItem)
    {
        var existingItem = await base.GetById(id);
        if(existingItem is not null)
        {
            updatedItem.LastCalibrationDate = existingItem.CalibrationDate;
        }
        return await base.UpdateAsync(id, updatedItem);
    }

}
