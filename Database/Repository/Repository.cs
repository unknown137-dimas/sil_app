using Database.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : ModelBase
{
    protected readonly LisDbContext _dbContext;
    public Repository(LisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<TEntity> GetEntities()
    {
        return _dbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetAsync(string id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    public async ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity)
    {
        return await _dbContext.Set<TEntity>().AddAsync(entity);
    }

    public EntityEntry<TEntity> Update(TEntity entity)
    {
        return _dbContext.Set<TEntity>().Update(entity);
    }

    public EntityEntry<TEntity> Delete(TEntity entity)
    {
        return _dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task<int> CommitAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public async void Dispose()
    {
        await _dbContext.DisposeAsync();
    }

    public bool IsExisted(string id)
    {
        return _dbContext.Set<TEntity>().FirstOrDefault(_ => _.Id.Equals(id)) is not null;
    }
}