using Backend.DTOs;
using Backend.Models;
using Backend.Modules;
using Backend.Utilities;
using Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/patient/check/result")]
public class PatientCheckResultController : ApiBaseController<PatientCheckResultController, PatientCheckResultDTO>
{
    private readonly Module<PatientCheckResultDTO, PatientCheckResult> _patientCheckResultModule;

    public PatientCheckResultController(
        ILogger<PatientCheckResultController> logger,
        Module<PatientCheckResultDTO, PatientCheckResult> patientCheckResultModule,
        IResponseFactory<PatientCheckResultDTO> responseFactory
    ) : base(logger, responseFactory)
    {
        _patientCheckResultModule = patientCheckResultModule;
    }


    [HttpGet()]
    public ActionResult<Response<PatientCheckResultDTO>> GetAllPatientCheckResult()
    {
        var item = _patientCheckResultModule.GetAll();
        return GeneratedResponse(item, "");
    }

    [HttpGet("{patientCheckResultId}")]
    public async Task<ActionResult<Response<PatientCheckResultDTO>>> GetPatientCheckResultByIdAsync(string patientCheckResultId)
    {
        var item = await _patientCheckResultModule.GetById(patientCheckResultId);
        var message = item is not null ? "" : "Item Not Found";
        return GeneratedResponse(item, message);
    }

    [HttpPost()]
    public async Task<ActionResult<Response<PatientCheckResultDTO>>> CreatePatientCheckResult(PatientCheckResultDTO newPatientCheckResult)
    {
        PatientCheckResultDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _patientCheckResultModule.AddAsync(newPatientCheckResult);
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

    [HttpPost("{patientCheckResultId}")]
    public async Task<ActionResult<Response<PatientCheckResultDTO>>> UpdatePatientCheckResult(string patientCheckResultId, PatientCheckResultDTO updatedPatientCheckResult)
    {
        PatientCheckResultDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _patientCheckResultModule.UpdateAsync(patientCheckResultId, updatedPatientCheckResult);
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

    [HttpDelete("{patientCheckResultId}")]
    public async Task<ActionResult<Response<PatientCheckResultDTO>>> DeletePatientCheckResult(string patientCheckResultId)
    {
        PatientCheckResultDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _patientCheckResultModule.DeleteAsync(patientCheckResultId);
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
