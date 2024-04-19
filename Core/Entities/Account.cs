using Core.Constants;
using Core.Interfaces;

namespace Core.Entities;

public class Account 
{
    public int Id { get; set; }
    public string Holder { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public EAccountType AccountType { get; set; } = EAccountType.Current;

    public decimal Balance { get; set; }


    public int CurrencyId { get; set; }
    public Currency Currency { get; set; } = null!;
    
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public EIsDeletedStatus IsDeleted { get; set; } = EIsDeletedStatus.False;
    public EAccountStatus AccountStatus { get; set; } = EAccountStatus.Active;


    public SavingAccount? SavingAccount { get; set; }
    public CurrentAccount? CurrentAccount { get; set; }


    public virtual ICollection<Movement> Movements { get; set; } = new List<Movement>();
}
