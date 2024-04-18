using Database.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class LisDbContext : IdentityDbContext, ILisDbContext
{
    public DbSet<User> IdentityUsers { get; set; }
    public DbSet<CheckCategory> CheckCategories { get; set; }
    public DbSet<CheckService> CheckServices { get; set; }
    public DbSet<MedicalTool> MedicalTools { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<PatientCheck> PatientChecks { get; set; }
    public DbSet<PatientCheckResult> PatientCheckResults { get; set; }
    public DbSet<PatientSample> PatientSamples { get; set; }
    public DbSet<PatientSampleResult> PatientSampleResults { get; set; }
    public DbSet<Reagen> Reagens { get; set; }
    public DbSet<SampleCategory> SampleCategories { get; set; }
    public DbSet<SampleService> SampleServices { get; set; }
    public DbSet<AuthAction> AuthActions { get; set; }
    public DbSet<RoleAuthAction> RoleAuthActions { get; set; }

    protected readonly IConfiguration _configuration;

    public LisDbContext(IConfiguration configuration, DbContextOptions<LisDbContext> options) : base(options)
    {
        _configuration = configuration;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(_ =>
        {
            _.Property(_ => _.FirstName).IsRequired(true);
            _.Property(_ => _.LastName).IsRequired(true);
        });
        modelBuilder.Entity<CheckCategory>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).ValueGeneratedOnAdd();
            _.Property(_ => _.Name).IsRequired(true).HasMaxLength(30);
            _.HasMany(_ => _.CheckServices);
            _.HasIndex(_ => _.Name).IsUnique();
        });
        modelBuilder.Entity<CheckService>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).ValueGeneratedOnAdd();
            _.Property(_ => _.Name).IsRequired(true).HasMaxLength(30);
            _.Property(_ => _.NormalValueType).IsRequired(true);
            _.Property(_ => _.CheckUnit).IsRequired(false).HasMaxLength(15);
            _.Property(_ => _.Gender).IsRequired(true);
            _.Property(_ => _.MinNormalValue).IsRequired(false);
            _.Property(_ => _.MaxNormalValue).IsRequired(false);
            _.Property(_ => _.NormalValue).IsRequired(false);
            _.HasOne(_ => _.CheckCategory);
            _.HasIndex(_ => _.Name).IsUnique();
        });
        modelBuilder.Entity<MedicalTool>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).ValueGeneratedOnAdd();
            _.Property(_ => _.Name).IsRequired(true).HasMaxLength(30);
            _.Property(_ => _.Code).IsRequired(true).HasMaxLength(20);
            _.Property(_ => _.LastCalibrationDate).IsRequired(true);
            _.Property(_ => _.CalibrationDate).IsRequired(true);
            _.Property(_ => _.CalibrationStatus).IsRequired(true);
            _.Property(_ => _.CalibrationNote).IsRequired(false);
            _.HasIndex(_ => _.Name).IsUnique();
            _.HasIndex(_ => _.Code).IsUnique();
        });
        modelBuilder.Entity<Patient>(_ =>
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).ValueGeneratedOnAdd();
            _.Property(_ => _.Name).IsRequired(true).HasMaxLength(30);
            _.Property(_ => _.MedicalRecordNumber).IsRequired(true);
            _.Property(_ => _.DateOfBirth).IsRequired(true);
            _.Property(_ => _.Gender).IsRequired(true);
            _.Property(_ => _.IdentityNumber).IsRequired(true);
            _.Property(_ => _.HealthInsuranceNumber).IsRequired(false);
            _.Property(_ => _.PhoneNumber).IsRequired(true).HasMaxLength(20);
            _.Property(_ => _.Address).IsRequired(true).HasMaxLength(100);
            _.HasIndex(_ => _.Name).IsUnique();
            _.HasIndex(_ => _.IdentityNumber).IsUnique();
            _.HasIndex(_ => _.MedicalRecordNumber).IsUnique();
            _.HasIndex(_ => _.HealthInsuranceNumber).IsUnique();
        });
        modelBuilder.Entity<PatientCheck>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).ValueGeneratedOnAdd();
            _.Property(_ => _.CheckSchedule).IsRequired(true);
            _.Property(_ => _.ValidationStatus).IsRequired(true);
            _.Property(_ => _.CheckStatus).IsRequired(true);
            _.HasOne(_ => _.CheckService);
            _.HasOne(_ => _.Patient);
        });
        modelBuilder.Entity<PatientCheckResult>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).ValueGeneratedOnAdd();
            _.Property(_ => _.NumericResult).IsRequired(false);
            _.Property(_ => _.StringResult).IsRequired(false);
            _.HasOne(_ => _.PatientCheck);
            _.HasOne(_ => _.CheckService);
        });
        modelBuilder.Entity<PatientSample>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).ValueGeneratedOnAdd();
            _.Property(_ => _.SampleSchedule).IsRequired(true);
            _.HasOne(_ => _.Patient);
            _.HasOne(_ => _.SampleService);
        });
        modelBuilder.Entity<PatientSampleResult>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).ValueGeneratedOnAdd();
            _.Property(_ => _.SampleTakenDate).IsRequired(true);
            _.Property(_ => _.SampleNote).IsRequired(false);
            _.HasOne(_ => _.PatientSample);
            _.HasOne(_ => _.SampleService);
        });
        modelBuilder.Entity<Reagen>(_ =>
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).ValueGeneratedOnAdd();
            _.Property(_ => _.Name).IsRequired(true).HasMaxLength(30);
            _.Property(_ => _.Code).IsRequired(true).HasMaxLength(20);
            _.Property(_ => _.ExpiredDate).IsRequired(true);
            _.Property(_ => _.Stock).IsRequired(true);
            _.HasIndex(_ => _.Name).IsUnique();
            _.HasIndex(_ => _.Code).IsUnique();
        });
        modelBuilder.Entity<SampleCategory>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).ValueGeneratedOnAdd();
            _.Property(_ => _.Name).IsRequired(true).HasMaxLength(30);
            _.HasMany(_ => _.SampleServices);
            _.HasIndex(_ => _.Name).IsUnique(); ;
        });
        modelBuilder.Entity<SampleService>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).ValueGeneratedOnAdd();
            _.Property(_ => _.Name).IsRequired(true).HasMaxLength(30);
            _.HasOne(_ => _.SampleCategory);
            _.HasIndex(_ => _.Name).IsUnique(); ;
        });
        modelBuilder.Entity<AuthAction>(_ =>
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Id).ValueGeneratedOnAdd();
            _.Property(_ => _.Action).IsRequired(true).HasMaxLength(30);
        });
        modelBuilder.Entity<RoleAuthAction>(_ =>
        {
        });
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}