using Database.Enums;
using Database.Models;

public class CheckService : ModelBase
{
    public string Name { get; set; } = null!;
    public Guid CheckCategoryId { get; set; }
    public CheckCategory CheckCategory { get; set; } = null!;
    public CheckType NormalValueType { get; set; }
    public string? CheckUnit { get; set; }
    public Gender Gender { get; set; }
    public float? MinNormalValue { get; set; }
    public float? MaxNormalValue { get; set; }
    public string? NormalValue { get; set; }
}