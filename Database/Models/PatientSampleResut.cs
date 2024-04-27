namespace Database.Models;

public class PatientSampleResult : ModelBase
{
    public DateTime SampleTakenDate { get; set; }
    public string PatientSampleId { get; set; } = null!;
    public PatientSample PatientSample { get; set; } = null!;
    public string SampleNote { get; set; } = null!;
}