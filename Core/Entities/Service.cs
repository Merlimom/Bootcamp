namespace Core.Entities;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

}
