using Database.Enums;

namespace Database.Models;

public class Patient : ModelBase
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