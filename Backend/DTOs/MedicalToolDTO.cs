using Backend.DTOs;
using Database.Enums;

public class MedicalToolDTO : DTOBase
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public CalibrationStatus CalibrationStatus { get; set; } = CalibrationStatus.NotOptimal;
}