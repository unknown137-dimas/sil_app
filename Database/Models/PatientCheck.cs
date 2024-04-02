using Database.Enums;
using Database.Models;

public class PatientCheck : ModelBase
{
    public DateTime CheckSchedule { get; set; }
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    public ICollection<CheckService> CheckServices { get; set; } = null!;
    public ValidationStatus ValidationStatus { get; set; } = ValidationStatus.NotValid;
    public CheckStatus CheckStatus { get; set; } = CheckStatus.Waiting;
}