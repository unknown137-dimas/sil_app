using Backend.DTOs;

namespace Backend.Modules.Interfaces;

public interface IMedicalToolModule
{
    Task<MedicalToolDTO?> GetById(string medicalToolId);
    Task<int> AddMedicalToolAsync(MedicalToolDTO newMedicalTool);
    Task<int> UpdateMedicalToolAsync(MedicalToolDTO updatedMedicalTool);
    Task<int> DeleteMedicalToolAsync(string medicalToolId);
}
