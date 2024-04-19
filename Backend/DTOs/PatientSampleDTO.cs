namespace Backend.DTOs;

public class PatientSampleDTO : DTOBase
{
    public DateTime SampleSchedule { get; set; }
    public string PatientId { get; set; } = null!;
    public IEnumerable<SampleServiceDTO> SampleServices { get; set; } = null!;
    public IEnumerable<PatientSampleResultDTO> PatientSampleResults { get; set; } = null!;
}