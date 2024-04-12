using Core.Constants;
using Core.Entities;
using Core.Models;
using Core.Request;
using Mapster;

namespace Infrastructure.Mappings;

public class AccountMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateAccountModel, CreateAccountModel>()
        .Map(dest => dest.Holder, src => src.Holder)
        .Map(dest => dest.Number, src => src.Number)
        .Map(dest => dest.AccountType, src => Enum.Parse<AccountType>(src.AccountType))
        .Map(dest => dest.CustomerId, src => src.CustomerId)
        .Map(dest => dest.CurrencyId, src => src.CurrencyId);

        config.NewConfig<Account, AccountDTO>()
        .Map(dest => dest.Holder, src => src.Holder)
        .Map(dest => dest.Number, src => src.Number)
        .Map(dest => dest.Balance, src => src.Balance)
        .Map(dest => dest.AccountStatus, src => src.AccountStatus)
        .Map(dest => dest.Customer, src => src.Customer)
        .Map(dest => dest.Currency, src => src.Currency);

    }
}
