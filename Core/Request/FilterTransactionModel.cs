namespace Core.Request;

public class FilterTransactionModel
{
    public int AccountId { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Description { get; set; }
    public string? MovementType { get; set; }
}
