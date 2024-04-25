namespace Core.Request;

public class CreateWithdrawalModel
{
    public decimal Amount { get; set; }

    public string? Description { get; set; }


    public DateTime WithdrawalDateTime { get; set; }

    public int AccountId { get; set; }

    public int BankId { get; set; }
}