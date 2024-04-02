namespace Database.Models;

public class ModelBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

}