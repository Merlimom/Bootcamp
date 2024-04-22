using Core.Entities;
using Core.Models;
using Core.Request;
using Mapster;

namespace Infrastructure.Mappings;

public class DepositMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateDepositModel, Deposit>()
          .Map(dest => dest.Amount, src => src.Amount)
          .Map(dest => dest.DepositDateTime, src => src.DepositDateTime)
          .Map(dest => dest.AccountId, src => src.AccountId);

        config.NewConfig<Deposit, DepositDTO>()
         .Map(dest => dest.Id, src => src.Id)
         .Map(dest => dest.Amount, src => src.Amount)
         .Map(dest => dest.DepositDateTime, src => src.DepositDateTime)
         .Map(dest => dest.Account, src => src.Account);

    }
}
