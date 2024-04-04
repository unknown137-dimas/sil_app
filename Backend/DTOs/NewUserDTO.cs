namespace Backend.DTOs;

public class NewUserDTO : UserDTO
{
    public string Password { get; set; } = null!;
}
