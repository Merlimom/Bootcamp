using Core.Constants;

namespace Core.Request;

public class CreateAccountModel
{
    public string Holder { get; set; } = string.Empty;

    public string Number { get; set; } = string.Empty;

    public string AccountType { get; set; } = string.Empty;

    public int CurrencyId { get; set; }

    public int CustomerId { get; set; }

    // Propiedades específicas de la cuenta de ahorro
    public string SavingType { get; set; } = string.Empty;

    public string HolderName { get; set; } = string.Empty;

    // Propiedades específicas de la cuenta corriente
    public decimal OperationalLimit { get; set; }
    public decimal MonthAverage { get; set; }
    public decimal Interest { get; set; }
}
