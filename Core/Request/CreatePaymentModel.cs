namespace Core.Request;

public class CreatePaymentModel
{
    public string DocumentNumber { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public int AccountId { get; set; }
    public int ServiceId { get; set; }
}