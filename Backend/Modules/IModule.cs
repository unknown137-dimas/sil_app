namespace Backend.Modules;

public interface IModule<DTO>
{
    abstract IEnumerable<DTO> GetAll();
    abstract Task<DTO?> GetById(string id);
    abstract Task<int> AddAsync(DTO newItem);
    abstract Task<int> UpdateAsync(DTO updatedItem);
    abstract Task<int> DeleteAsync(string id);
    abstract bool IsExisted(string id);
}