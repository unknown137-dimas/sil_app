namespace Backend.DTOs;

public class UserDTO : DTOBase
{
	public string UserName { get; set; } = null!;
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public RoleDTO Role { get; set; } = null!;
}