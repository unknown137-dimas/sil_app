namespace Backend.Models;

public class Response<TEntity>
{
    public string Message { get; set; } = null!;
    public IEnumerable<object>? Messages { get; set; }
    public IEnumerable<TEntity> Data { get; set; } = null!;
}