using Core.Constants;

namespace Core.Request;

public class FilterAccountModel
{
    public string? Number { get; set; }
    public AccountType? AccountType { get; set; }
    public int? CustomerId { get; set; }
    public int? CurrencyId { get; set; }

}
