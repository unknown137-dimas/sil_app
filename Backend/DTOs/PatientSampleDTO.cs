namespace Backend.DTOs;

public class PatientSampleDTO : DTOBase
{
    public DateTime SampleSchedule { get; set; }
    public string PatientId { get; set; } = null!;
    public string SampleServiceId { get; set; } = null!;
}