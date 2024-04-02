using Database.Models;

public class SampleCategory : ModelBase
{
    public string Name { get; set; } = null!;
    public ICollection<SampleService>? SampleServices { get; set; }

    public SampleCategory()
    {
        SampleServices = new HashSet<SampleService>();
    }
}