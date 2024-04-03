namespace Database.Models;

public class SampleService : ModelBase
{
    public string Name { get; set; } = null!;
    public string SampleCategoryId { get; set; } = null!;
    public SampleCategory SampleCategory { get; set; } = null!;
}