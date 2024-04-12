using Core.Constants;
using Core.Entities;

namespace Core.Models;

public class SavingAccountDTO
{
    public int Id { get; set; }

    public SavingType SavingType { get; set; } = SavingType.Permanent;

    public AccountDTO Account { get; set; } = null!;
}
