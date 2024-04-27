namespace Database.Models;

public class PatientSample : ModelBase
{
    public DateTime SampleSchedule { get; set; }
    public string PatientId { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public string SampleServiceId { get; set; } = null!;
    public SampleService SampleService { get; set; } = null!;
}