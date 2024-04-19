using Core.Constants;
using Core.Entities;
using Core.Models;
using Core.Request;

namespace Core.Interfaces.Repositories;

public interface IMovementRepository
{
    Task<MovementDTO>Add(CreateMovementModel model);
    Task<List<MovementDTO>> GetAll();
    //Task<(AccountType AccountTypeSource, Currency CurrencySource, decimal Balance,
    //    AccountStatus AccountStatus, Bank BankSource, decimal? OperationalLimit)?>
    //    GetAccountInformation(int accountId);
    Task<bool> ValidateTransfer(int accountSourceId, int accountDestinationId, decimal amount);


}
