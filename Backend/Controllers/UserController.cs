using AutoMapper;
using Backend.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UserController(
        ILogger<UserController> logger,
        UserManager<User> userManager,
        IMapper mapper
    )
    {
        _logger = logger;
        _userManager = userManager;
        _mapper = mapper;
    }

     [HttpGet()]
    public ActionResult<IEnumerable<UserDTO>> GetAllUser()
    {
        var item = _userManager.Users;
        return Ok(_mapper.Map<IEnumerable<UserDTO>>(item));
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDTO?>> GetUserByIdAsync(string userId)
    {
        var item = await _userManager.FindByIdAsync(userId);
        return item is not null ? Ok(_mapper.Map<UserDTO>(item)) : NotFound();
    }

    [HttpPost()]
    public async Task<ActionResult<IdentityResult>> CreateUser(NewUserDTO newUser)
    {
        var newItem = new User()
        {
            UserName = newUser.UserName,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            EmailConfirmed = true

        };
        var result = await _userManager.CreateAsync(newItem, newUser.Password);
        if(result.Succeeded)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost("{userId}")]
    public async Task<ActionResult<IdentityResult>> UpdateUser(string userId, UserDTO updatedUser)
    {
        IdentityResult result = new IdentityResult();
        var existingUser = await _userManager.FindByIdAsync(userId);
        if(existingUser is not null)
        {
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Email = updatedUser.Email;
            existingUser.UserName = updatedUser.UserName;
            result = await _userManager.UpdateAsync(existingUser);
        }
        if(result.Succeeded)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult<IdentityResult>> DeleteUser(string userId)
    {
        var deletedItem = await _userManager.FindByIdAsync(userId);
        IdentityResult result = new IdentityResult();
        if(deletedItem is not null)
        {
            result = await _userManager.DeleteAsync(deletedItem);
        }
        if(result.Succeeded)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
