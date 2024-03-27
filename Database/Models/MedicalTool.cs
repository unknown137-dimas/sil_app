public class MedicalTool : ModelBase
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public DateTime LastCalibrationDate { get; set; }
    public DateTime CalibrationDate { get; set; }
    public CalibrationStatus CalibrationStatus { get; set; } = CalibrationStatus.NotOptimal;
    public string? CalibrationNote { get; set; }
}