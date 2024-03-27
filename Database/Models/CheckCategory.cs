public class CheckCategory : ModelBase
{
    public string Name { get; set; } = null!;
    public ICollection<CheckService>? CheckServices { get; set; }

    public CheckCategory()
    {
        CheckServices = new HashSet<CheckService>();
    }
}