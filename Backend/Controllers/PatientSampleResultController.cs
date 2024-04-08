using Backend.DTOs;
using Backend.Models;
using Backend.Modules;
using Backend.Utilities;
using Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/patient/sample/result")]
public class PatientSampleResultController : ApiBaseController<PatientSampleResultController, PatientSampleResultDTO>
{
    private readonly Module<PatientSampleResultDTO, PatientSampleResult> _patientSampleResultModule;

    public PatientSampleResultController(
        ILogger<PatientSampleResultController> logger,
        Module<PatientSampleResultDTO, PatientSampleResult> patientSampleResultModule,
        IResponseFactory<PatientSampleResultDTO> responseFactory
    ) : base(logger, responseFactory)
    {
        _patientSampleResultModule = patientSampleResultModule;
    }


    [HttpGet()]
    public ActionResult<Response<PatientSampleResultDTO>> GetAllPatientSampleResult()
    {
        var item = _patientSampleResultModule.GetAll();
        return GeneratedResponse(item, "");
    }

    [HttpGet("{patientSampleResultId}")]
    public async Task<ActionResult<Response<PatientSampleResultDTO>>> GetPatientSampleResultByIdAsync(string patientSampleResultId)
    {
        var item = await _patientSampleResultModule.GetById(patientSampleResultId);
        var message = item is not null ? "" : "Item Not Found";
        return GeneratedResponse(item, message);
    }

    [HttpPost()]
    public async Task<ActionResult<Response<PatientSampleResultDTO>>> CreatePatientSampleResult(PatientSampleResultDTO newPatientSampleResult)
    {
        PatientSampleResultDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _patientSampleResultModule.AddAsync(newPatientSampleResult);
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

    [HttpPost("{patientSampleResultId}")]
    public async Task<ActionResult<Response<PatientSampleResultDTO>>> UpdatePatientSampleResult(string patientSampleResultId, PatientSampleResultDTO updatedPatientSampleResult)
    {
        PatientSampleResultDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _patientSampleResultModule.UpdateAsync(patientSampleResultId, updatedPatientSampleResult);
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

    [HttpDelete("{patientSampleResultId}")]
    public async Task<ActionResult<Response<PatientSampleResultDTO>>> DeletePatientSampleResult(string patientSampleResultId)
    {
        PatientSampleResultDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _patientSampleResultModule.DeleteAsync(patientSampleResultId);
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
