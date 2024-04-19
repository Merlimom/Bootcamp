using Core.Constants;

namespace Core.Request;

public class CreateAccountModel
{
    public string Holder { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public int CurrencyId { get; set; }
    public int CustomerId { get; set; }
    public EAccountType AccountType { get; set; }
    public CreateSavingAccountModel? CreateSavingAccountModel { get; set; }
    public CreateCurrentAccountModel? CreateCurrentAccountModel { get; set; }
}
