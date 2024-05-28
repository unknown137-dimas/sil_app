using Backend.DTOs;
using Backend.Models;
using Backend.Modules;
using Backend.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/patient/sample")]
public class PatientSampleController : ApiBaseController<PatientSampleController, PatientSampleDTO>
{
    private readonly PatientSampleModule _patientSampleModule;

    public PatientSampleController(
        ILogger<PatientSampleController> logger,
        PatientSampleModule patientSampleModule,
        IResponseFactory<PatientSampleDTO> responseFactory
    ) : base(logger, responseFactory)
    {
        _patientSampleModule = patientSampleModule;
    }


    [HttpGet()]
    public ActionResult<Response<PatientSampleDTO>> GetAllPatientSample()
    {
        var item = _patientSampleModule.GetAll();
        return GeneratedResponse(item, "");
    }

    [HttpGet("{patientSampleId}")]
    public async Task<ActionResult<Response<PatientSampleDTO>>> GetPatientSampleByIdAsync(string patientSampleId)
    {
        var item = await _patientSampleModule.GetById(patientSampleId);
        var message = item is not null ? "" : "Item Not Found";
        return GeneratedResponse(item, message);
    }

    [HttpPost()]
    public async Task<ActionResult<Response<PatientSampleDTO>>> CreatePatientSample(PatientSampleDTO newPatientSample)
    {
        PatientSampleDTO? item = null;
        string message = string.Empty;
        try
        {
            if (newPatientSample.SampleNote != null)
            {
                var noteArray = newPatientSample.SampleNote.ToLower().Split(" ");
                noteArray[0] = textInfo.ToTitleCase(noteArray[0]);
                newPatientSample.SampleNote = string.Join(" ", noteArray);
            }
            item = await _patientSampleModule.AddAsync(newPatientSample);
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

    [HttpPost("{patientSampleId}")]
    public async Task<ActionResult<Response<PatientSampleDTO>>> UpdatePatientSample(string patientSampleId, PatientSampleDTO updatedPatientSample)
    {
        PatientSampleDTO? item = null;
        string message = string.Empty;
        try
        {
            if (updatedPatientSample.SampleNote != null)
            {
                var noteArray = updatedPatientSample.SampleNote.ToLower().Split(" ");
                noteArray[0] = textInfo.ToTitleCase(noteArray[0]);
                updatedPatientSample.SampleNote = string.Join(" ", noteArray);
            }
            item = await _patientSampleModule.UpdateAsync(patientSampleId, updatedPatientSample);
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

    [HttpDelete("{patientSampleId}")]
    public async Task<ActionResult<Response<PatientSampleDTO>>> DeletePatientSample(string patientSampleId)
    {
        PatientSampleDTO? item = null;
        string message = string.Empty;
        try
        {
            item = await _patientSampleModule.DeleteAsync(patientSampleId);
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
