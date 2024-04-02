using Microsoft.EntityFrameworkCore.ChangeTracking;

public interface IRepository<TEntity> where TEntity : class
{
    public IQueryable<TEntity> GetEntities();
    public Task<TEntity?> GetAsync(Guid id);
    public ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity);
    public EntityEntry<TEntity> Update(TEntity entity);
    public EntityEntry<TEntity> Delete(TEntity entity);
    public Task<int> CommitAsync();
}