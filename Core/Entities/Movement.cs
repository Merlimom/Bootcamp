using Core.Constants;

namespace Core.Entities;

public class Movement
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime TransferredDateTime { get; set; }

    public ETransferStatus TransferStatus { get; set; } = ETransferStatus.Pending;

    public int AccountDestinationId { get; set; }

    public int AccountSourceId { get; set; }

    public virtual Account Account { get; set; } = null!;
}
