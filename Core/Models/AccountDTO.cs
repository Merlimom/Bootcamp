using Core.Constants;
using Core.Entities;

namespace Core.Models;

public class AccountDTO
{
    public int Id { get; set; }
    public string Holder { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public EAccountType AccountType { get; set; } = EAccountType.Current;
    public decimal Balance { get; set; }
    public string AccountStatus { get; set; } = string.Empty;
    public string Currency { get; set; } = null!;
    public string Customer { get; set; } = null!;

    public SavingAccountDTO? SavingAccount { get; set; }
    public CurrentAccountDTO? CurrentAccount { get; set; }
}