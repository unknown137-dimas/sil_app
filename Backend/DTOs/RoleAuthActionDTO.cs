namespace Backend.DTOs;

public class RoleAuthActionDTO : DTOBase
{
    public string RoleId { get; set; } = null!;
    public RoleDTO Role { get; set; } = null!;
    public string AuthActionId { get; set; } = null!;
    public AuthActionDTO AuthAction { get; set; } = null!;
}
