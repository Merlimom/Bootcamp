namespace Core.Entities;

public class Promotion
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DurationTime { get; set; }
    public decimal PercentageOff { get; set; }

    public int BusinessId { get; set; }
    public Business Business { get; set; } = null!;
}
