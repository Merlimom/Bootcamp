using Core.Constants;

namespace Core.Entities;

public class SavingAccount
{
    public int Id { get; set; }


    public ESavingType SavingType { get; set; } = ESavingType.Insight;

    public string HolderName { get; set; } = string.Empty;

    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;
}
