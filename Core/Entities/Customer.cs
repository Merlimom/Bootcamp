using Core.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Lastname { get; set; }

    public string DocumentNumber { get; set; } = string.Empty;

    public string? Address { get; set; }

    public string? Mail { get; set; }

    public string? Phone { get; set; }

    public ECustomerStatus CustomerStatus { get; set; } = ECustomerStatus.Active;

    public int BankId { get; set; }

    public DateTime? Birth { get; set; }

    public virtual Bank Bank { get; set; } = null!;

    public ICollection<Account> Accounts { get; set; } = new List<Account>();

    public ICollection<CreditCard> CreditCards { get; set; } = new List<CreditCard>();

    public ICollection<UserRequest> UserRequests { get; set; } = new List<UserRequest>();


}
