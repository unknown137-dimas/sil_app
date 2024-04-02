namespace Backend.DTOs;

public class CheckCategoryDTO : DTOBase
{
    public string Name { get; set; } = null!;
    public IEnumerable<CheckServiceDTO> CheckServices { get; set; } = null!;
}