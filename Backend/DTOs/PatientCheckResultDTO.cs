namespace Backend.DTOs;

public class PatientCheckResultDTO : DTOBase
{
    public string PatientCheckId { get; set; } = null!;
    public PatientCheckDTO PatientCheck { get; set; } = null!;
    public string CheckServiceId { get; set; } = null!;
    public CheckServiceDTO CheckService { get; set; } = null!;
    public float? NumericResult { get; set; }
    public string? StringResult { get; set; }
}