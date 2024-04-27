using AutoMapper;
using Backend.DTOs;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Modules;

public class PatientCheckModule : Module<PatientCheckDTO, PatientCheck>
{
    private readonly IRelationCheckerModule _relationCheckerModule;

    public PatientCheckModule(
        IRepository<PatientCheck> repository,
        IMapper mapper,
        IRelationCheckerModule relationCheckerModule) : base(repository, mapper)
    {
        _relationCheckerModule = relationCheckerModule;
    }

    public override async Task<PatientCheckDTO?> DeleteAsync(string id)
    {
        var patientCheck = await Repository.GetAsync(id);
        if(patientCheck is not null && _relationCheckerModule.Check(patientCheck) is not null)
        {
            throw new Exception("Can't delete because linked to other data");
        }
        return await base.DeleteAsync(id);
    }

}
