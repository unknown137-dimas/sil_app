using Backend.DTOs;
using Backend.Models;
using Backend.Modules;
using Backend.Utilities;
using Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/patient")]
public class PatientController : ApiBaseController<PatientController, PatientDTO>
{
    private readonly Module<PatientDTO, Patient> _patientModule;

    public PatientController(
        ILogger<PatientController> logger,
        Module<PatientDTO, Patient> patientModule,
        IResponseFactory<PatientDTO> responseFactory
    ) : base(logger, responseFactory)
    {
        _patientModule = patientModule;
    }


    [HttpGet()]
    public ActionResult<Response<PatientDTO>> GetAllPatient()
    {
        var item = _patientModule.GetAll();
        return GeneratedResponse(item, "");
    }

    [HttpGet("{patientId}")]
    public async Task<ActionResult<Response<PatientDTO>>> GetPatientByIdAsync(string patientId)
    {
        var item = await _patientModule.GetById(patientId);
        var message = item is not null ? "" : "Item Not Found";
        return GeneratedResponse(item, message);
    }

    [HttpPost()]
    public async Task<ActionResult<Response<PatientDTO>>> CreatePatient(PatientDTO newPatient)
    {
        PatientDTO? item = null;
        string message = string.Empty;
        try
        {
            newPatient.Name = textInfo.ToTitleCase(newPatient.Name.ToLower());
            newPatient.Address = textInfo.ToTitleCase(newPatient.Address.ToLower());
            item = await _patientModule.AddAsync(newPatient);
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

    [HttpPost("{patientId}")]
    public async Task<ActionResult<Response<PatientDTO>>> UpdatePatient(string patientId, PatientDTO updatedPatient)
    {
        PatientDTO? item = null;
        string message = string.Empty;
        try
        {
            updatedPatient.Name = textInfo.ToTitleCase(updatedPatient.Name.ToLower());
            updatedPatient.Address = textInfo.ToTitleCase(updatedPatient.Address.ToLower());
            item = await _patientModule.UpdateAsync(patientId, updatedPatient);
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

    [HttpDelete("{patientId}")]
    public async Task<ActionResult<Response<PatientDTO>>> DeletePatient(string patientId)
    {
        PatientDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _patientModule.DeleteAsync(patientId);
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
