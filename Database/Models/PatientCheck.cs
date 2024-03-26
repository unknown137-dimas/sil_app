public class PatientCheck
{
    public Guid Id { get; set; }
    public DateTime CheckSchedule { get; set; }
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    public Guid CheckServiceId { get; set; }
    public CheckService CheckService { get; set; } = null!;
    public ValidationStatus ValidationStatus { get; set; } = ValidationStatus.NotValid;
    public CheckStatus CheckStatus { get; set; } = CheckStatus.Waiting;
}