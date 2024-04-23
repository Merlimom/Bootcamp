namespace Core.Models;

public class WithdrawalDTO
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime DepositDateTime { get; set; }

    public AccountDTO Account { get; set; } = null!;
}