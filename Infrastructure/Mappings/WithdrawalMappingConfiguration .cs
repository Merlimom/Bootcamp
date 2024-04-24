using Core.Entities;
using Core.Models;
using Core.Request;
using Mapster;

namespace Infrastructure.Mappings;

public class WithdrawalMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateWithdrawalModel, Withdrawal>()
          .Map(dest => dest.Amount, src => src.Amount)
          .Map(dest => dest.WithdrawalDateTime, src => src.WithdrawalDateTime)
          .Map(dest => dest.AccountId, src => src.AccountId);

        config.NewConfig<Withdrawal, WithdrawalDTO>()
         .Map(dest => dest.Id, src => src.Id)
         .Map(dest => dest.Amount, src => src.Amount)
         .Map(dest => dest.WithdrawalDateTime, src => src.WithdrawalDateTime)
         .Map(dest => dest.Account, src => src.Account);
    }
}