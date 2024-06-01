namespace Backend.DTOs;

public class PatientCheckRequestDTO
{
    public DateTime CheckSchedule { get; set; }
    public string PatientId { get; set; } = null!;
}