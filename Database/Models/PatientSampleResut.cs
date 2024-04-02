namespace Database.Models;

public class PatientSampleResult : ModelBase
{
    public DateTime SampleTakenDate { get; set; }
    public Guid PatientSampleId { get; set; }
    public PatientSample PatientSample { get; set; } = null!;
    public Guid SampleServiceId { get; set; }
    public SampleService SampleService { get; set; } = null!;
    public string SampleNote { get; set; } = null!;
}