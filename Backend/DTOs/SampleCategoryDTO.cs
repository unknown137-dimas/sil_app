namespace Backend.DTOs;

public class SampleCategoryDTO : DTOBase
{
    public string Name { get; set; } = null!;
    public IEnumerable<SampleServiceDTO>? SampleServices { get; set; }
}