using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using Backend.Modules;
using Backend.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/role")]
public class RoleController : ApiBaseController<RoleController, RoleDTO>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly IRelationCheckerModule _relationCheckerModule;

    public RoleController(
        ILogger<RoleController> logger,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper,
        IResponseFactory<RoleDTO> responseFactory,
        IRelationCheckerModule relationCheckerModule
    ) : base(logger, responseFactory)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _relationCheckerModule = relationCheckerModule;
    }

    [HttpGet()]
    public ActionResult<Response<RoleDTO>> GetAllRole()
    {
        var item = _mapper.Map<IEnumerable<RoleDTO>>(_roleManager.Roles);
        return GeneratedResponse(item, "");
    }

    [HttpGet("{roleId}")]
    public async Task<ActionResult<Response<RoleDTO>>> GetRoleByIdAsync(string roleId)
    {
        var item = _mapper.Map<RoleDTO>(await _roleManager.FindByIdAsync(roleId));
        var message = item is not null ? "" : "Item Not Found";
        return GeneratedResponse(item, message);
    }

    [HttpPost()]
    public async Task<ActionResult<Response<RoleDTO>>> CreateRole(RoleDTO newRole)
    {
        var newItem = new IdentityRole()
        {
            Name = newRole.Name
        };
        var result = await _roleManager.CreateAsync(newItem);
        if(result.Succeeded)
        {
            var roleCreated = _mapper.Map<RoleDTO>(_roleManager.FindByNameAsync(newRole.Name));
            return GeneratedResponse(roleCreated, result.Errors);
        }
        return GeneratedResponse(null, result.Errors);
    }

    [HttpPost("{roleId}")]
    public async Task<ActionResult<Response<RoleDTO>>> UpdateRole(string roleId, RoleDTO updatedRole)
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
            var roleUpdated = _mapper.Map<RoleDTO>(_roleManager.FindByIdAsync(roleId));
            return GeneratedResponse(roleUpdated, result.Errors);
        }
        return GeneratedResponse(null, result.Errors);
    }

    [HttpDelete("{roleId}")]
    public async Task<ActionResult<Response<RoleDTO>>> DeleteRole(string roleId)
    {
        var deletedRole = await _roleManager.FindByIdAsync(roleId);
        IdentityResult result = new IdentityResult();
        if(deletedRole is not null && _relationCheckerModule.Check(deletedRole) is null)
        {
            result = await _roleManager.DeleteAsync(deletedRole);
        }
        if(result.Succeeded)
        {
            var roleDeleted = _mapper.Map<RoleDTO>(await _roleManager.FindByIdAsync(roleId));
            return GeneratedResponse(roleDeleted, result.Errors);
        }
        return GeneratedResponse(null, result.Errors);
    }
}
