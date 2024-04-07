namespace Backend.Models;

public class Response<TEntity>
{
    public IEnumerable<object>? Messages { get; set; }
    public IEnumerable<TEntity> Data { get; set; } = null!;
}