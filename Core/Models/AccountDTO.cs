using Core.Constants;
using Core.Entities;

namespace Core.Models;

public class AccountDTO
{
    public int Id { get; set; }

    public string Holder { get; set; } = string.Empty;

    public string Number { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public AccountType AccountType { get; set; } = AccountType.Current;

    public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;

    public CurrencyDTO Currency { get; set; } = null!;
    public CustomerDTO Customer { get; set; } = null!;
    public SavingAccountDTO SavingAccount { get; set; } = null!;
    public CurrentAccountDTO CurrentAccount { get; set; } = null!;

}

