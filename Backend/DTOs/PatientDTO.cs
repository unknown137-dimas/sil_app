using Database.Enums;

namespace Backend.DTOs;

public class PatientDTO : DTOBase
{
    public string Name { get; set; } = null!;
    public string MedicalRecordNumber { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string IdentityNumber { get; set; } = null!;
    public string? HealthInsuranceNumber { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
}