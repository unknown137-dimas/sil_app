namespace Database.Models;

public class PatientSample : ModelBase
{
    public DateTime SampleSchedule { get; set; }
    public string PatientId { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public ICollection<SampleService> SampleServices { get; set; } = null!;
    public ICollection<PatientSampleResult> PatientSampleResults { get; set; } = null!;

    public PatientSample()
    {
        SampleServices = new HashSet<SampleService>();
        PatientSampleResults = new HashSet<PatientSampleResult>();
    }
}