using Core.Entities;
using Core.Models;
using Core.Request;
using Mapster;

namespace Infrastructure.Mappings;

internal class AccountTransactionMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Movement, AccountTransactionsDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Type, src => "Movement")
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.TransactionDateTime, src => src.TransferredDateTime)
            .Map(dest => dest.AccountId, src => src.AccountSourceId)
            .Map(dest => dest.AccountId, src => src.AccountDestinationId);


        config.NewConfig<Payment, AccountTransactionsDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Type, src => "Payment")
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.TransactionDateTime, src => src.DepositDateTime)//cambiar a paymentdatetime
            .Map(dest => dest.AccountId, src => src.AccountId);

        config.NewConfig<Deposit, AccountTransactionsDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Type, src => "Deposit")
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.TransactionDateTime, src => src.DepositDateTime)
            .Map(dest => dest.AccountId, src => src.AccountId);

        config.NewConfig<Withdrawal, AccountTransactionsDTO>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Type, src => "Withdrawal")
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.TransactionDateTime, src => src.WithdrawalDateTime)
            .Map(dest => dest.AccountId, src => src.AccountId);

    }
}
