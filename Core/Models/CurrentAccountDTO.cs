using Core.Constants;
using Core.Entities;

namespace Core.Models; 

public class CurrentAccountDTO
{
    public int Id { get; set; }
    public decimal OperationalLimit { get; set; }
    public decimal MonthAverage { get; set; }
    public decimal Interest { get; set; }

    public AccountDTO Account { get; set; } = null!;


    //public CurrencyDTO Currency { get; set; }
    //public CustomerDTO Customer { get; set; }
}
