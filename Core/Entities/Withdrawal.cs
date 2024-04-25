namespace Core.Entities;

public class Withdrawal
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime WithdrawalDateTime { get; set; }

    public int AccountId { get; set; }

    public Account Account { get; set; } = null!;
}