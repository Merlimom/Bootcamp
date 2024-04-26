namespace Core.Models;

public class AccountTransactionsDTO
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime TransactionDateTime { get; set; }

    public int AccountId { get; set; } 
}
