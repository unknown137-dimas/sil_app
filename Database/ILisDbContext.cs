using Database.Models;
using Microsoft.EntityFrameworkCore;

public interface ILisDbContext
{
    public DbSet<User> IdentityUsers { get; set; }
    public DbSet<CheckCategory> CheckCategories { get; set; }
    public DbSet<CheckService> CheckServices { get; set; }
    public DbSet<MedicalTool> MedicalTools { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<PatientCheck> PatientChecks { get; set; }
    public DbSet<PatientSample> PatientSamples { get; set; }
    public DbSet<Reagen> Reagens { get; set; }
    public DbSet<SampleCategory> SampleCategories { get; set; }
    public DbSet<SampleService> SampleServices { get; set; }
    public DbSet<AuthAction> AuthActions { get; set; }
    public DbSet<RoleAuthAction> RoleAuthActions { get; set; }
    public Task<int> SaveChangesAsync();
}