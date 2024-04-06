using Backend.DTOs;
using Backend.Modules;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/medicaltool")]
public class MedicalToolController : ControllerBase
{
    private readonly ILogger<MedicalToolController> _logger;
    private readonly MedicalToolModule _medicalToolModule;

    public MedicalToolController(
        ILogger<MedicalToolController> logger,
        MedicalToolModule medicalToolModule
    )
    {
        _logger = logger;
        _medicalToolModule = medicalToolModule;
    }


    [HttpGet()]
    public ActionResult<IEnumerable<MedicalToolDTO>> GetAllMedicalTool()
    {
        var item = _medicalToolModule.GetAll();
        return Ok(item);
    }

    [HttpGet("{medicalToolId}")]
    public async Task<ActionResult<MedicalToolDTO?>> GetMedicalToolByIdAsync(string medicalToolId)
    {
        var item = await _medicalToolModule.GetById(medicalToolId);
        return item is not null ? Ok(item) : NotFound();
    }

    [HttpPost()]
    public async Task<ActionResult<int>> CreateMedicalTool(MedicalToolDTO newMedicalTool)
    {
        var item = await _medicalToolModule.AddAsync(newMedicalTool);
        return item > 0 ? Ok(item) : BadRequest();
    }

    [HttpPost("{medicalToolId}")]
    public async Task<ActionResult<MedicalToolDTO>> UpdateMedicalTool(string medicalToolId, MedicalToolDTO updatedMedicalTool)
    {
        int item = 0;
        if(_medicalToolModule.IsExisted(medicalToolId))
        {
            item = await _medicalToolModule.UpdateAsync(updatedMedicalTool);
        }
        return item > 0 ? Ok(updatedMedicalTool) : BadRequest("Failed to update");
    }

    [HttpDelete("{medicalToolId}")]
    public async Task<ActionResult<int>> DeleteMedicalTool(string medicalToolId)
    {
        var item = await _medicalToolModule.DeleteAsync(medicalToolId);
        return item > 0 ? Ok(item) : BadRequest();
    }
}
