using Backend.DTOs;
using Backend.Models;
using Backend.Modules;
using Backend.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/medical-tool")]
public class MedicalToolController : ApiBaseController<MedicalToolController, MedicalToolDTO>
{
    private readonly MedicalToolModule _medicalToolModule;

    public MedicalToolController(
        ILogger<MedicalToolController> logger,
        MedicalToolModule medicalToolModule,
        IResponseFactory<MedicalToolDTO> responseFactory
    ) : base(logger, responseFactory)
    {
        _medicalToolModule = medicalToolModule;
    }


    [HttpGet()]
    public ActionResult<Response<MedicalToolDTO>> GetAllMedicalTool()
    {
        var item = _medicalToolModule.GetAll();
        return GeneratedResponse(item, "");
    }

    [HttpGet("{medicalToolId}")]
    public async Task<ActionResult<Response<MedicalToolDTO>>> GetMedicalToolByIdAsync(string medicalToolId)
    {
        var item = await _medicalToolModule.GetById(medicalToolId);
        var message = item is not null ? "" : "Item Not Found";
        return GeneratedResponse(item, message);
    }

    [HttpPost()]
    public async Task<ActionResult<Response<MedicalToolDTO>>> CreateMedicalTool(MedicalToolDTO newMedicalTool)
    {
        MedicalToolDTO? item = null;
        string message = string.Empty;
        try
        {
            newMedicalTool.Name = textInfo.ToTitleCase(newMedicalTool.Name.ToLower());
            if (newMedicalTool.CalibrationNote != null)
            {
                var noteArray = newMedicalTool.CalibrationNote.ToLower().Split(" ");
                noteArray[0] = textInfo.ToTitleCase(noteArray[0]);
                newMedicalTool.CalibrationNote = string.Join(" ", noteArray);
            }
            item = await _medicalToolModule.AddAsync(newMedicalTool);
        }
        catch (Exception ex)
        {
            message = ex.Message;
            if(ex.InnerException is not null)
            {
                message = ex.InnerException.Message;
            }
        }
        return GeneratedResponse(item, message);
    }

    [HttpPost("{medicalToolId}")]
    public async Task<ActionResult<Response<MedicalToolDTO>>> UpdateMedicalTool(string medicalToolId, MedicalToolDTO updatedMedicalTool)
    {
        MedicalToolDTO? item = null;
        string message = string.Empty;
        try
        {
            updatedMedicalTool.Name = textInfo.ToTitleCase(updatedMedicalTool.Name.ToLower());
            if (updatedMedicalTool.CalibrationNote != null)
            {
                var noteArray = updatedMedicalTool.CalibrationNote.ToLower().Split(" ");
                noteArray[0] = textInfo.ToTitleCase(noteArray[0]);
                updatedMedicalTool.CalibrationNote = string.Join(" ", noteArray);
            }
            item = await _medicalToolModule.UpdateAsync(medicalToolId, updatedMedicalTool);
        }
        catch (Exception ex)
        {
            message = ex.Message;
            if(ex.InnerException is not null)
            {
                message = ex.InnerException.Message;
            }
        }
        return GeneratedResponse(item, message);
    }

    [HttpDelete("{medicalToolId}")]
    public async Task<ActionResult<Response<MedicalToolDTO>>> DeleteMedicalTool(string medicalToolId)
    {
        MedicalToolDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _medicalToolModule.DeleteAsync(medicalToolId);
        }
        catch (Exception ex)
        {
            message = ex.Message;
            if(ex.InnerException is not null)
            {
                message = ex.InnerException.Message;
            }
        }
        return GeneratedResponse(item, message);
    }
}
