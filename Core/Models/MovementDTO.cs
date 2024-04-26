using Core.Constants;
using Core.Entities;

namespace Core.Models;

public class MovementDTO
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime? TransferredDateTime { get; set; }

    public ETransferStatus TransferStatus { get; set; } = ETransferStatus.Pending;

    public string AccountDestination { get; set; } = string.Empty;

    public string AccountSource { get; set; } = string.Empty;

    public AccountDTO Account { get; set; } = null!;
}
