namespace Core.Request;

public class UpdateAccountModel
{
    public int Id { get; set; }

    public string Holder { get; set; } = string.Empty;

    public string Number { get; set; } = string.Empty;

    public Decimal Balance { get; set; }

    public string AccountStatus { get; set; } = string.Empty;

    public int CurrencyId { get; set; }

    public int CustomerId { get; set; }
}
