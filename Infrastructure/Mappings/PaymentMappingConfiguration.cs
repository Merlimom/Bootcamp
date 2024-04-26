
using Core.Entities;
using Core.Models;
using Core.Request;
using Mapster;

namespace Infrastructure.Mappings;

public class PaymentMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreatePaymentModel, Payment>()
           .Map(dest => dest.DocumentNumber, src => src.DocumentNumber)
           .Map(dest => dest.Description, src => src.Description)
           .Map(dest => dest.Amount, src => src.Amount)
           .Map(dest => dest.DepositDateTime, src => src.PaymentDateTime)
           .Map(dest => dest.AccountId, src => src.AccountId)
           .Map(dest => dest.ServiceId, src => src.ServiceId);

        config.NewConfig<Payment, PaymentDTO>()
           .Map(dest => dest.Id, src => src.Id)
           .Map(dest => dest.DocumentNumber, src => src.DocumentNumber)
           .Map(dest => dest.Description, src => src.Description)
           .Map(dest => dest.PaymentDateTime, src => src.DepositDateTime)
           .Map(dest => dest.Amount, src => src.Amount)
           .Map(dest => dest.Account, src => src.Account)
           .Map(dest => dest.Service, src => src.Service);
    }
}
