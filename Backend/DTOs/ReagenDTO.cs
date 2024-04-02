namespace Backend.DTOs;

public class ReagenDTO : DTOBase
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public DateTime ExpiredDate { get; set; }
    public int Stock { get; set; }
}