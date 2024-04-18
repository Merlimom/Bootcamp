using Core.Constants;
using Core.Models;

namespace Core.Request;

public class CreateMovementModel
{
    public EMovementType MovementType { get; set; } = EMovementType.Transfer;

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime? TransferredDateTime { get; set; }

    public TransferStatus TransferStatus { get; set; } = TransferStatus.Pending;

    public int AccountDestinationId { get; set; }

    public int AccountSourceId { get; set; }
}
