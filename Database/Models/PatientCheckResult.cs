public class PatientCheckResult : ModelBase
{
    public Guid PatientCheckId { get; set; }
    public PatientCheck PatientCheck { get; set; } = null!;
    public Guid CheckServiceId { get; set; }
    public CheckService CheckService { get; set; } = null!;
    public float? NumericResult { get; set; }
    public string? StringResult { get; set; }
}