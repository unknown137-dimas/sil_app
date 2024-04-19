using Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
    private readonly IRepository<AuthAction> _authActionRepository;
    private readonly IRepository<RoleAuthAction> _roleAuthActionRepository;

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
        IRepository<PatientSampleResult> patientSampleResultRepository,
        IRepository<AuthAction> authActionRepository,
        IRepository<RoleAuthAction> roleAuthActionRepository
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
        _authActionRepository = authActionRepository;
        _roleAuthActionRepository = roleAuthActionRepository;
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
        var relationToAuthActionExist = _roleAuthActionRepository.GetEntities()
                .FirstOrDefault(ra => ra.RoleId.Equals(role.Id)) is not null;
        return relationToUserExist || relationToAuthActionExist ? role : null;
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
            .Include(pc => pc.CheckServices)
            .SelectMany(pc => pc.CheckServices)
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
            .Include(pc => pc.SampleServices)
            .SelectMany(pc => pc.SampleServices)
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
        return _patientCheckResultRepository.GetEntities()
            .Where(pc => pc.PatientCheckId.Equals(patientCheck.Id))
            .FirstOrDefault();
    }

    public ModelBase? Check(PatientCheckResult patientCheckResult)
    {
        throw new NotImplementedException();
    }

    public ModelBase? Check(PatientSample patientSample)
    {
        return _patientSampleResultRepository.GetEntities()
            .Where(pc => pc.PatientSampleId.Equals(patientSample.Id))
            .FirstOrDefault();
    }

    public ModelBase? Check(PatientSampleResult patientSampleResult)
    {
        throw new NotImplementedException();
    }

    public ModelBase? Check(AuthAction authAction)
    {
        return _roleAuthActionRepository.GetEntities()
            .FirstOrDefault(ra => ra.AuthActionId == authAction.Id);
    }
}
