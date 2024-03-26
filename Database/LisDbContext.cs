using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class LisDbContext : IdentityDbContext, ILisDbContext
{
    public DbSet<Patient> Patients { get; set; }

    public LisDbContext(){}
    public LisDbContext(DbContextOptions<LisDbContext> options) : base(options) {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("FileName=./Lis.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Patient>(_ =>
        {
            _.HasKey(_ => _.Id);
            _.Property(_ => _.Name).IsRequired(true).HasMaxLength(30);
            _.Property(_ => _.Address).IsRequired(true).HasMaxLength(100);
        });
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}