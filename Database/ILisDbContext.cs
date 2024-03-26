using Microsoft.EntityFrameworkCore;

public interface ILisDbContext
{
    public DbSet<Patient> Patients { get; set; }
    public Task<int> SaveChangesAsync();
}