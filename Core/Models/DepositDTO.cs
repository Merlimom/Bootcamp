using Core.Entities;

namespace Core.Models;

public class DepositDTO
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime DepositDateTime { get; set; }

    public Account Account { get; set; } = null!;

}
