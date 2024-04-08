namespace Backend.DTOs;

public class PatientSampleResultDTO : DTOBase
{
    public DateTime SampleTakenDate { get; set; }
    public string PatientSampleId { get; set; } = null!;
    public PatientSampleDTO PatientSample { get; set; } = null!;
    public string SampleServiceId { get; set; } = null!;
    public SampleServiceDTO SampleService { get; set; } = null!;
    public string SampleNote { get; set; } = null!;
}