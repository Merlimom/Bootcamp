
namespace Core.Models;

public class PaymentDTO
{
    public int Id { get; set; }

    public string DocumentNumber { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public ServiceDTO Service { get; set; } = null!;

    public AccountDTO Account { get; set; } = null!;
}
