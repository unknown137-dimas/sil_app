public class PatientCheckResult
{
    public Guid Id { get; set; }
    public Guid PatientCheckId { get; set; }
    public PatientCheck PatientCheck { get; set; } = null!;
    public float? NumericResult { get; set; }
    public string? StringResult { get; set; }
}