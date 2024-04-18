using Core.Constants;
using Core.Entities;

namespace Core.Models;

public class UserRequestDTO
{
    public int Id { get; set; }

    public DateTime RequestDate { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public string RequestStatus { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Product { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public string Customer { get; set; } = null!;


}
