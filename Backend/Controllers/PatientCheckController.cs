using Backend.DTOs;
using Backend.Models;
using Backend.Modules;
using Backend.Utilities;
using Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/patient/check")]
public class PatientCheckController : ApiBaseController<PatientCheckController, PatientCheckDTO>
{
    private readonly PatientCheckModule _patientCheckModule;
    private readonly Module<PatientDTO, Patient> _patientModule;

    public PatientCheckController(
        ILogger<PatientCheckController> logger,
        PatientCheckModule patientCheckModule,
        Module<PatientDTO, Patient> patientModule,
        IResponseFactory<PatientCheckDTO> responseFactory
    ) : base(logger, responseFactory)
    {
        _patientCheckModule = patientCheckModule;
        _patientModule = patientModule;
    }


    [HttpGet()]
    public ActionResult<Response<PatientCheckDTO>> GetAllPatientCheck()
    {
        var item = _patientCheckModule.GetAll();
        return GeneratedResponse(item, "");
    }

    [HttpGet("{patientCheckId}")]
    public async Task<ActionResult<Response<PatientCheckDTO>>> GetPatientCheckByIdAsync(string patientCheckId)
    {
        var item = await _patientCheckModule.GetById(patientCheckId);
        var message = item is not null ? "" : "Item Not Found";
        return GeneratedResponse(item, message);
    }

    [HttpPost()]
    public async Task<ActionResult<Response<PatientCheckDTO>>> CreatePatientCheck(PatientCheckDTO newPatientCheck)
    {
        PatientCheckDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _patientCheckModule.AddAsync(newPatientCheck);
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

    [HttpPost("{patientCheckId}")]
    public async Task<ActionResult<Response<PatientCheckDTO>>> UpdatePatientCheck(string patientCheckId, PatientCheckDTO updatedPatientCheck)
    {
        PatientCheckDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _patientCheckModule.UpdateAsync(patientCheckId, updatedPatientCheck);
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

    [HttpDelete("{patientCheckId}")]
    public async Task<ActionResult<Response<PatientCheckDTO>>> DeletePatientCheck(string patientCheckId)
    {
        PatientCheckDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _patientCheckModule.DeleteAsync(patientCheckId);
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

    [HttpGet("export-pdf")]
    public ActionResult ExportPdf([FromQuery] PatientCheckRequestDTO requestDTO)
    {
        var patientName = _patientModule.GetById(requestDTO.PatientId).GetAwaiter().GetResult()?.Name;
        var fileName = $"Result_{patientName}_{requestDTO.CheckSchedule:dd_MM_yyyy}.pdf";
        return File(_patientCheckModule.ExportPdf(requestDTO), "application/pdf", fileName);
    }
}
