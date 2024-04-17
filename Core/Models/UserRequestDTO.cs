using Core.Constants;
using Core.Entities;

namespace Core.Models;

public class UserRequestDTO
{
    public int Id { get; set; }

    public DateTime RequestDate { get; set; }

    public DateTime ApprovalDate { get; set; }

    public string RequestStatus { get; set; } = string.Empty;

    public ProductDTO Product { get; set; } = null!;

    public CurrencyDTO Currency { get; set; } = null!;

    public CustomerDTO Customer { get; set; } = null!;

}
