using Database.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public interface IRepository<TEntity> where TEntity : ModelBase
{
    public IQueryable<TEntity> GetEntities();
    public Task<TEntity?> GetAsync(string id);
    public ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity);
    public EntityEntry<TEntity> Update(TEntity entity);
    public EntityEntry<TEntity> Delete(TEntity entity);
    public Task<int> CommitAsync();
    public bool IsExisted(string id);
}