using Database.Models;
using Microsoft.AspNetCore.Identity;

namespace Backend.Modules;

public class RelationCheckerModule : IRelationCheckerModule
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IRepository<CheckCategory> _checkCategoryRepository;
    private readonly IRepository<CheckService> _checkServiceRepository;
    private readonly IRepository<SampleCategory> _sampleCategoryRepository;
    private readonly IRepository<SampleService> _sampleServiceRepository;
    private readonly IRepository<Patient> _patientRepository;
    private readonly IRepository<PatientCheck> _patientCheckRepository;
    private readonly IRepository<PatientSample> _patientSampleRepository;

    public RelationCheckerModule(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IRepository<CheckCategory> checkCategoryRepository,
        IRepository<CheckService> checkServiceRepository,
        IRepository<SampleCategory> sampleCategoryRepository,
        IRepository<SampleService> sampleServiceRepository,
        IRepository<Patient> patientRepository,
        IRepository<PatientCheck> patientCheckRepository,
        IRepository<PatientSample> patientSampleRepository
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _checkCategoryRepository = checkCategoryRepository;
        _checkServiceRepository = checkServiceRepository;
        _sampleCategoryRepository = sampleCategoryRepository;
        _sampleServiceRepository = sampleServiceRepository;
        _patientRepository = patientRepository;
        _patientCheckRepository = patientCheckRepository;
        _patientSampleRepository = patientSampleRepository;
    }


    public IdentityUser? Check(IdentityUser user)
    {
        throw new NotImplementedException();
    }

    public IdentityRole? Check(IdentityRole role)
    {
        var relationToUserExist = _userManager.GetUsersInRoleAsync(role.Name!)
            .Result
            .Count > 0;
        return relationToUserExist ? role : null;
    }

    public ModelBase? Check(CheckCategory checkCategory)
    {
        return _checkServiceRepository.GetEntities()
            .Where(cs => cs.CheckCategoryId.Equals(checkCategory.Id))
            .FirstOrDefault();
    }

    public ModelBase? Check(CheckService checkService)
    {
        return _patientCheckRepository.GetEntities()
            .Where(cs => cs.Id.Equals(checkService.Id))
            .FirstOrDefault();
    }

    public ModelBase? Check(SampleCategory sampleCategory)
    {
        return _sampleServiceRepository.GetEntities()
            .Where(cs => cs.SampleCategoryId.Equals(sampleCategory.Id))
            .FirstOrDefault();
    }

    public ModelBase? Check(SampleService sampleService)
    {
        return _patientSampleRepository.GetEntities()
            .Where(cs => cs.Id.Equals(sampleService.Id))
            .FirstOrDefault();
    }

    public ModelBase? Check(Patient patient)
    {
        var patientCheck = _patientCheckRepository.GetEntities()
            .Where(pc => pc.PatientId.Equals(patient.Id))
            .FirstOrDefault();

        if (patientCheck != null) return patientCheck;

        var patientsample = _patientSampleRepository.GetEntities()
            .Where(pc => pc.PatientId.Equals(patient.Id))
            .FirstOrDefault();

        return patientsample;
    }

    public ModelBase? Check(PatientCheck patientCheck)
    {
        return null;
    }

    public ModelBase? Check(PatientSample patientSample)
    {
        return null;
    }
}
