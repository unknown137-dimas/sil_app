using Backend.DTOs;

namespace Backend.Modules.Interfaces;

public interface IReagenModule
{
    Task<ReagenDTO?> GetById(string reagenId);
    Task<int> AddReagenAsync(ReagenDTO newReagen);
    Task<int> UpdateReagenAsync(ReagenDTO updatedReagen);
    Task<int> DeleteReagenAsync(string reagenId);
}