using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class LisDbContext : IdentityDbContext, ILisDbContext
{
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

    public LisDbContext(){}
    public LisDbContext(DbContextOptions<LisDbContext> options) : base(options) {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("FileName=./App.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<CheckCategory>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Name).IsRequired(true).HasMaxLength(30);
            _.HasMany(_ => _.CheckServices);
        });
        modelBuilder.Entity<CheckService>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Name).IsRequired(true).HasMaxLength(30);
            _.Property(_ => _.NormalValueType).IsRequired(true);
            _.Property(_ => _.CheckUnit).IsRequired(false).HasMaxLength(15);
            _.Property(_ => _.Gender).IsRequired(true);
            _.Property(_ => _.MinNormalValue).IsRequired(false);
            _.Property(_ => _.MaxNormalValue).IsRequired(false);
            _.Property(_ => _.NormalValue).IsRequired(false);
            _.HasOne(_ => _.CheckCategory);
        });
        modelBuilder.Entity<MedicalTool>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Name).IsRequired(true).HasMaxLength(30);
            _.Property(_ => _.Code).IsRequired(true).HasMaxLength(20);
            _.Property(_ => _.LastCalibrationDate).IsRequired(true);
            _.Property(_ => _.CalibrationDate).IsRequired(true);
            _.Property(_ => _.CalibrationStatus).IsRequired(true);
            _.Property(_ => _.CalibrationNote).IsRequired(false);
        });
        modelBuilder.Entity<Patient>(_ =>
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Name).IsRequired(true).HasMaxLength(30);
            _.Property(_ => _.MedicalRecordNumber).IsRequired(true);
            _.Property(_ => _.DateOfBirth).IsRequired(true);
            _.Property(_ => _.Gender).IsRequired(true);
            _.Property(_ => _.IdentityNumber).IsRequired(true);
            _.Property(_ => _.HealthInsuranceNumber).IsRequired(false);
            _.Property(_ => _.PhoneNumber).IsRequired(true).HasMaxLength(20);
            _.Property(_ => _.Address).IsRequired(true).HasMaxLength(100);
        });
        modelBuilder.Entity<PatientCheck>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.CheckSchedule).IsRequired(true);
            _.Property(_ => _.ValidationStatus).IsRequired(true);
            _.Property(_ => _.CheckStatus).IsRequired(true);
            _.HasMany(_ => _.CheckServices);
            _.HasOne(_ => _.Patient);
        });
        modelBuilder.Entity<PatientCheckResult>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.NumericResult).IsRequired(false);
            _.Property(_ => _.StringResult).IsRequired(false);
            _.HasOne(_ => _.PatientCheck);
            _.HasOne(_ => _.CheckService);
        });
        modelBuilder.Entity<PatientSample>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.SampleSchedule).IsRequired(true);
            _.HasOne(_ => _.Patient);
            _.HasMany(_ => _.SampleServices);
        });
        modelBuilder.Entity<PatientSampleResult>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.SampleTakenDate).IsRequired(true);
            _.Property(_ => _.SampleNote).IsRequired(false);
            _.HasOne(_ => _.PatientSample);
            _.HasOne(_ => _.SampleService);
        });
        modelBuilder.Entity<Reagen>(_ =>
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Name).IsRequired(true).HasMaxLength(30);
            _.Property(_ => _.Code).IsRequired(true).HasMaxLength(20);
            _.Property(_ => _.ExpiredDate).IsRequired(true);
            _.Property(_ => _.Stock).IsRequired(true);
        });
        modelBuilder.Entity<SampleCategory>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Name).IsRequired(true).HasMaxLength(30);
            _.HasMany(_ => _.SampleServices);
        });
        modelBuilder.Entity<SampleService>(_ => 
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Name).IsRequired(true).HasMaxLength(30);
            _.HasOne(_ => _.SampleCategory);
        });
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}