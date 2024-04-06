using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using Backend.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ApiBaseController<UserController, UserDTO>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UserController(
        ILogger<UserController> logger,
        UserManager<User> userManager,
        IMapper mapper,
        IResponseFactory<UserDTO> responseFactory
    ) : base(logger, responseFactory)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

     [HttpGet()]
    public ActionResult<Response<UserDTO>> GetAllUser()
    {
        var item = _mapper.Map<IEnumerable<UserDTO>>(_userManager.Users);
        return GeneratedResponse(item, "");
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<Response<UserDTO>>> GetUserByIdAsync(string userId)
    {
        var item = _mapper.Map<UserDTO>(await _userManager.FindByIdAsync(userId));
        var message = item is not null ? "" : "Item Not Found";
        return GeneratedResponse(item, message);
    }

    [HttpPost()]
    public async Task<ActionResult<Response<UserDTO>>> CreateUser(NewUserDTO newUser)
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
            var userCreated = _mapper.Map<UserDTO>(_userManager.FindByNameAsync(newUser.UserName));
            return GeneratedResponse(userCreated, result.Errors);
        }
        return GeneratedResponse(null, result.Errors);
    }

    [HttpPost("{userId}")]
    public async Task<ActionResult<Response<UserDTO>>> UpdateUser(string userId, UserDTO updatedUser)
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
            var userUpdated = _mapper.Map<UserDTO>(await _userManager.FindByIdAsync(userId));
            return GeneratedResponse(userUpdated, result.Errors);
        }
        return GeneratedResponse(null, result.Errors);
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult<Response<UserDTO>>> DeleteUser(string userId)
    {
        var deletedItem = await _userManager.FindByIdAsync(userId);
        IdentityResult result = new IdentityResult();
        if(deletedItem is null)
        {
            return GeneratedResponse(null, result.Errors);
        }
        result = await _userManager.DeleteAsync(deletedItem);
        if(result.Succeeded)
        {
            var userDeleted = _mapper.Map<UserDTO>(await _userManager.FindByIdAsync(userId));
            return GeneratedResponse(userDeleted, result.Errors);
        }
        return GeneratedResponse(null, result.Errors);
    }
}
