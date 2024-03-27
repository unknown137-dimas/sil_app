public class PatientSample : ModelBase
{
    public DateTime SampleSchedule { get; set; }
    public Guid PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    public ICollection<SampleService> SampleServices { get; set; } = null!;
}