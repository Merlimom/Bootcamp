using Core.Constants;
using Core.Models;

namespace Core.Request;

public class CreateMovementModel
{
    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime? TransferredDateTime { get; set; }

    public int AccountDestinationId { get; set; }

    public int AccountSourceId { get; set; }

    public string? DestinationAccountNumber { get; set; }
    public string?DestinationDocumentNumber { get; set; }

    public int CurrencyId { get; set; }
    public int DestinationBankId { get; set; }
}
