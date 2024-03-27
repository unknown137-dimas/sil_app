public class SampleService : ModelBase
{
    public string Name { get; set; } = null!;
    public Guid SampleCategoryId { get; set; }
    public SampleCategory SampleCategory { get; set; } = null!;
}