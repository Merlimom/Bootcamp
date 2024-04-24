namespace Core.Models;

public class AccountTransactionsDTO
{
    public int Id { get; set; }
    public string Type { get; set; } 
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransferredDateTime { get; set; }

    public int AccountId { get; set; } 
}
