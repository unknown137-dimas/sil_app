using Database.Enums;

namespace Backend.DTOs;

public class PatientCheckDTO : DTOBase
{
    public DateTime CheckSchedule { get; set; }
    public string PatientId { get; set; } = null!;
    public IEnumerable<CheckServiceDTO> CheckServices { get; set; } = null!;
    public IEnumerable<PatientCheckResultDTO> PatientCheckResults { get; set; } = null!;
    public ValidationStatus ValidationStatus { get; set; } = ValidationStatus.NotValid;
    public CheckStatus CheckStatus { get; set; } = CheckStatus.Waiting;
}