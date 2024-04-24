namespace Core.Entities;

public class Withdrawal
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime WithdrawalDateTime { get; set; }

    public int AccountId { get; set; }

    public Account Account { get; set; } = null!;
}