using Microsoft.AspNetCore.Identity;

namespace Database.Models;

public class RoleAuthAction : ModelBase
{
    public string RoleId { get; set; } = null!;
    public IdentityRole Role { get; set; } = null!;
    public string AuthActionId { get; set; } = null!;
    public AuthAction AuthAction { get; set; } = null!;
}
