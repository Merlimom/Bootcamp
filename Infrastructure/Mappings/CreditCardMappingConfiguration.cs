using Core.Constants;
using Core.Entities;
using Core.Models;
using Core.Request;
using Mapster;

namespace Infrastructure.Mappings;

public class CreditCardMappingConfiguration : IRegister
{

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCreditCardModel, CreditCard>()
            .Map(dest => dest.Designation, src => src.Designation)
            .Map(dest => dest.IssueDate, src => src.IssueDate)
            .Map(dest => dest.ExpirationDate, src => src.ExpirationDate)
            .Map(dest => dest.CardNumber, src => src.CardNumber)
            .Map(dest => dest.Cvv, src => src.Cvv)
            .Map(dest => dest.CreditCardStatus, src => Enum.Parse<ECreditCardStatus>(src.CreditCardStatus))
            .Map(dest => dest.CreditLimit, src => src.CreditLimit)
            .Map(dest => dest.AvailableCredit, src => src.AvailableCredit)
            .Map(dest => dest.CurrentDebt, src => src.CurrentDebt)
            .Map(dest => dest.InterestRate, src => src.InterestRate)
            .Map(dest => dest.CustomerId, src => src.CustomerId)
            .Map(dest => dest.CurrencyId, src => src.CurrencyId);



        config.NewConfig<CreditCard, CreditCardDTO>()
            .Map(dest => dest.Designation, src => src.Designation)
            .Map(dest => dest.IssueDate, src => src.IssueDate)
            .Map(dest => dest.ExpirationDate, src => src.ExpirationDate)
            .Map(dest => dest.CardNumber, src => src.CardNumber)
            .Map(dest => dest.Cvv, src => src.Cvv)
            .Map(dest => dest.CreditCardStatus, src => src.CreditCardStatus)
            .Map(dest => dest.CreditLimit, src => src.CreditLimit)
            .Map(dest => dest.AvailableCredit, src => src.AvailableCredit)
            .Map(dest => dest.CurrentDebt, src => src.CurrentDebt)
            .Map(dest => dest.InterestRate, src => src.InterestRate)
            .Map(dest => dest.Customer, src => $"{src.Customer.Name} {src.Customer.Lastname}")
            .Map(dest => dest.Currency, src => src.Currency.Name);
    }
}
