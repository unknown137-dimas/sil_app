using AutoMapper;
using Backend.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/role")]
public class RoleController : ControllerBase
{
    private readonly ILogger<RoleController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public RoleController(
        ILogger<RoleController> logger,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper
    )
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    [HttpGet()]
    public ActionResult<IEnumerable<RoleDTO>> GetAllRole()
    {
        var item = _roleManager.Roles;
        return Ok(_mapper.Map<IEnumerable<RoleDTO>>(item));
    }

    [HttpGet("{roleId}")]
    public async Task<ActionResult<RoleDTO?>> GetRoleByIdAsync(string roleId)
    {
        var item = await _roleManager.FindByIdAsync(roleId);
        return item is not null ? Ok(_mapper.Map<RoleDTO>(item)) : NotFound();
    }

    [HttpPost()]
    public async Task<ActionResult<IdentityResult>> CreateRole(RoleDTO newRole)
    {
        var newItem = new IdentityRole()
        {
            Name = newRole.Name
        };
        var result = await _roleManager.CreateAsync(newItem);
        if(result.Succeeded)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost("{roleId}")]
    public async Task<ActionResult<IdentityResult>> UpdateRole(string roleId, RoleDTO updatedRole)
    {
        IdentityResult result = new IdentityResult();
        var existingRole = await _roleManager.FindByIdAsync(roleId);
        if(existingRole is not null)
        {
            existingRole.Name = updatedRole.Name;
            existingRole.NormalizedName = updatedRole.Name.ToUpper();
            result = await _roleManager.UpdateAsync(existingRole);
        }
        if(result.Succeeded)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete("{roleId}")]
    public async Task<ActionResult<IdentityResult>> DeleteRole(string roleId)
    {
        var deletedItem = await _roleManager.FindByIdAsync(roleId);
        IdentityResult result = new IdentityResult();
        if(deletedItem is not null && _userManager.GetUsersInRoleAsync(deletedItem.Name!).Result.Count.Equals(0))
        {
            result = await _roleManager.DeleteAsync(deletedItem);
        }
        if(result.Succeeded)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
