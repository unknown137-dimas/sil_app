namespace Backend.DTOs;

public class PatientSampleDTO : DTOBase
{
    public DateTime SampleSchedule { get; set; }
    public string PatientId { get; set; } = null!;
    public PatientDTO Patient { get; set; } = null!;
    public SampleServiceDTO SampleService { get; set; } = null!;
}