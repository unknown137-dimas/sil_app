namespace Backend.Models;

public class Response<TEntity>
{
    public string Message { get; set; } = null!;
    public IEnumerable<TEntity> Data { get; set; } = null!;
}