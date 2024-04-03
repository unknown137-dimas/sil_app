namespace Database.Models;

public class PatientCheckResult : ModelBase
{
    public string PatientCheckId { get; set; } = null!;
    public PatientCheck PatientCheck { get; set; } = null!;
    public string CheckServiceId { get; set; } = null!;
    public CheckService CheckService { get; set; } = null!;
    public float? NumericResult { get; set; }
    public string? StringResult { get; set; }
}