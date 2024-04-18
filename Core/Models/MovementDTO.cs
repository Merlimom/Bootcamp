using Core.Constants;
using Core.Entities;

namespace Core.Models;

public class MovementDTO
{
    public int Id { get; set; }

    public EMovementType MovementType { get; set; } = EMovementType.Transfer;

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime? TransferredDateTime { get; set; }

    public TransferStatus TransferStatus { get; set; } = TransferStatus.Pending;

    public int AccountDestinationId { get; set; }

    public int AccountSourceId { get; set; }

    public AccountDTO Account { get; set; } = null!;
}
