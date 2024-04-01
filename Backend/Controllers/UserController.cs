using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserController(
        ILogger<UserController> logger,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }
}
