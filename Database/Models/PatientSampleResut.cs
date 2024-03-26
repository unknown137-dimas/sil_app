public class PatientSampleResult
{
    public Guid Id { get; set; }
    public DateTime SampleTakenDate { get; set; }
    public Guid PatientSampleId { get; set; }
    public PatientSample PatientSample { get; set; } = null!;
    public string SampleNote { get; set; } = null!;
}