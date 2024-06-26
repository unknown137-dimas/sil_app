using AutoMapper;
using Backend.DTOs;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Modules;

public class PatientSampleModule : Module<PatientSampleDTO, PatientSample>
{
    private readonly IRelationCheckerModule _relationCheckerModule;

    public PatientSampleModule(
        IRepository<PatientSample> repository, 
        IMapper mapper,
        IRelationCheckerModule relationCheckerModule) : base(repository, mapper)
    {
        _relationCheckerModule = relationCheckerModule;
    }

    public override async Task<PatientSampleDTO?> DeleteAsync(string id)
    {
        var patientSample = await Repository.GetAsync(id);
        if(patientSample is not null && _relationCheckerModule.Check(patientSample) is not null)
        {
            throw new Exception("Can't delete because linked to other data");
        }
        return await base.DeleteAsync(id);
    }

}
