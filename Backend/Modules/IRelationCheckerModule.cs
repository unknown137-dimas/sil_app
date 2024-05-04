using Database.Models;
using Microsoft.AspNetCore.Identity;

namespace Backend.Modules;

public interface IRelationCheckerModule
{
    IdentityUser? Check(IdentityUser user);
    IdentityRole? Check(IdentityRole role);
    ModelBase? Check(CheckCategory checkCategory);
    ModelBase? Check(CheckService checkService);
    ModelBase? Check(SampleCategory sampleCategory);
    ModelBase? Check(SampleService sampleService);
    ModelBase? Check(Patient patient);
    ModelBase? Check(PatientCheck patientCheck);
    ModelBase? Check(PatientSample patientSample);
}
