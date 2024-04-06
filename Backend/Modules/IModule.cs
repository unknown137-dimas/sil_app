namespace Backend.Modules;

public interface IModule<DTO>
{
    abstract IEnumerable<DTO> GetAll();
    abstract Task<DTO?> GetById(string id);
    abstract Task<DTO?> AddAsync(DTO newItem);
    abstract Task<DTO?> UpdateAsync(DTO updatedItem);
    abstract Task<DTO?> DeleteAsync(string id);
    abstract bool IsExisted(string id);
}