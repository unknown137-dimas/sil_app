public class SampleService : ModelBase
{
    public Guid SampleCategoryId { get; set; }
    public SampleCategory SampleCategory { get; set; } = null!;
}