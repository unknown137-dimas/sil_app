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
    private readonly IRepository<PatientCheckResult> _patientCheckResultRepository;
    private readonly IRepository<PatientSample> _patientSampleRepository;
    private readonly IRepository<PatientSampleResult> _patientSampleResultRepository;

    public RelationCheckerModule(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IRepository<CheckCategory> checkCategoryRepository,
        IRepository<CheckService> checkServiceRepository,
        IRepository<SampleCategory> sampleCategoryRepository,
        IRepository<SampleService> sampleServiceRepository,
        IRepository<Patient> patientRepository,
        IRepository<PatientCheck> patientCheckRepository,
        IRepository<PatientCheckResult> patientCheckResultRepository,
        IRepository<PatientSample> patientSampleRepository,
        IRepository<PatientSampleResult> patientSampleResultRepository
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
        _patientCheckResultRepository = patientCheckResultRepository;
        _patientSampleRepository = patientSampleRepository;
        _patientSampleResultRepository = patientSampleResultRepository;
    }


    public IdentityUser? Check(IdentityUser user)
    {
        throw new NotImplementedException();
    }

    public IdentityRole? Check(IdentityRole role)
    {
        throw new NotImplementedException();
    }

    public ModelBase? Check(CheckCategory checkCategory)
    {
        return _checkServiceRepository.GetEntities()
            .Where(cs => cs.CheckCategoryId.Equals(checkCategory.Id))
            .FirstOrDefault();
    }

    public ModelBase? Check(CheckService checkService)
    {
        throw new NotImplementedException();
    }

    public ModelBase? Check(SampleCategory sampleCategory)
    {
        return _sampleServiceRepository.GetEntities()
            .Where(cs => cs.SampleCategoryId.Equals(sampleCategory.Id))
            .FirstOrDefault();
    }

    public ModelBase? Check(SampleService sampleService)
    {
        throw new NotImplementedException();
    }

    public ModelBase? Check(Patient patient)
    {
        throw new NotImplementedException();
    }

    public ModelBase? Check(PatientCheck patientCheck)
    {
        throw new NotImplementedException();
    }

    public ModelBase? Check(PatientCheckResult patientCheckResult)
    {
        throw new NotImplementedException();
    }

    public ModelBase? Check(PatientSample patientSample)
    {
        throw new NotImplementedException();
    }

    public ModelBase? Check(PatientSampleResult patientSampleResult)
    {
        throw new NotImplementedException();
    }

}
