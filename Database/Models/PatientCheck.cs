using Database.Enums;

namespace Database.Models;

public class PatientCheck : ModelBase
{
    public DateTime CheckSchedule { get; set; }
    public string PatientId { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public ValidationStatus ValidationStatus { get; set; } = ValidationStatus.NotValid;
    public CheckStatus CheckStatus { get; set; } = CheckStatus.Waiting;
    public string CheckServiceId { get; set; } = null!;
    public CheckService CheckService { get; set; } = null!;
}