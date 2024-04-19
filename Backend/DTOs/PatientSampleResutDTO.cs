namespace Backend.DTOs;

public class PatientSampleResultDTO : DTOBase
{
    public DateTime SampleTakenDate { get; set; }
    public string PatientSampleId { get; set; } = null!;
    public string SampleServiceId { get; set; } = null!;
    public string SampleNote { get; set; } = null!;
}