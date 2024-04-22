using Core.Constants;
using Core.Entities;
using Core.Models;
using Core.Request;

namespace Core.Interfaces.Repositories;

public interface IMovementRepository
{
    Task<MovementDTO> Add(CreateMovementModel model);
    Task<List<MovementDTO>> GetAll();
    Task<bool> IsSameAccountType(int sourceAccountId, int destinationAccountId);
    Task<bool> IsSameCurrency(int sourceAccountId, int destinationAccountId);
    Task<bool> IsSufficientBalance(int sourceAccountId, decimal amount);
    Task<bool> IsSourceAccountActive(int sourceAccountId);
    Task<(bool, string)> ExceedsOperationalLimit(int sourceAccountId, int destinationAccountId, decimal amount);
    Task<bool> IsSameBank(int sourceAccountId, int destinationAccountId);
    Task UpdateAccountBalancesAndLimits(int sourceAccountId, int destinationAccountId, decimal amount);
    Task<bool> ExceedsOperationalLimitForCurrentAccount(int destinationAccountId, decimal amount);
    //Task ProcessDeposit(CreateMovementModel model);
    //Task ProcessWithdrawal(CreateMovementModel model);

}
