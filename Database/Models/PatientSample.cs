public class PatientSample
{
    public Guid Id { get; set; }
    public DateTime SampleSchedule { get; set; }
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    public Guid SampleServiceId { get; set; }
    public SampleService SampleService { get; set; } = null!;
}