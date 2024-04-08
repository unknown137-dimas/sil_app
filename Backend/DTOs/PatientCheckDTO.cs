using Database.Enums;

namespace Backend.DTOs;

public class PatientCheckDTO : DTOBase
{
    public DateTime CheckSchedule { get; set; }
    public string PatientId { get; set; } = null!;
    public PatientDTO Patient { get; set; } = null!;
    public CheckServiceDTO CheckService { get; set; } = null!;
    public ValidationStatus ValidationStatus { get; set; } = ValidationStatus.NotValid;
    public CheckStatus CheckStatus { get; set; } = CheckStatus.Waiting;
}