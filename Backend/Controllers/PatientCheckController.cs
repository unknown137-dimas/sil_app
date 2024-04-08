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
    private readonly Module<PatientCheckDTO, PatientCheck> _patientCheckModule;

    public PatientCheckController(
        ILogger<PatientCheckController> logger,
        Module<PatientCheckDTO, PatientCheck> patientCheckModule,
        IResponseFactory<PatientCheckDTO> responseFactory
    ) : base(logger, responseFactory)
    {
        _patientCheckModule = patientCheckModule;
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
}
