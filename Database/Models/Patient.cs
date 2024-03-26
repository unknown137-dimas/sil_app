public class Patient : ModelBase
{
    public string MedicalRecordNumber { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string IdentityNumber { get; set; } = null!;
    public string HealthInsuranceNumber { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
}