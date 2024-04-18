using Core.Constants;

namespace Core.Entities;

public class UserRequest
{
    public int Id { get; set; }

    public DateTime RequestDate { get; set; }

    public DateTime? ApprovalDate { get; set; } 

    public string Description { get; set; } = string.Empty;

    public RequestStatus RequestStatus { get; set; } = RequestStatus.Pending;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int CurrencyId { get; set; }
    public Currency Currency { get; set; } = null!;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

}
