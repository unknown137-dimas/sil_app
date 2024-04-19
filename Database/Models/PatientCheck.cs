using Database.Enums;

namespace Database.Models;

public class PatientCheck : ModelBase
{
    public DateTime CheckSchedule { get; set; }
    public string PatientId { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public ICollection<CheckService> CheckServices { get; set; } = null!;
    public ValidationStatus ValidationStatus { get; set; } = ValidationStatus.NotValid;
    public CheckStatus CheckStatus { get; set; } = CheckStatus.Waiting;
    public ICollection<PatientCheckResult>? PatientCheckResults { get; set; }

    public PatientCheck()
    {
        CheckServices = new HashSet<CheckService>();
        PatientCheckResults = new HashSet<PatientCheckResult>();
    }
}