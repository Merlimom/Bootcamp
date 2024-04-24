using Core.Models;
using Core.Request;

namespace Core.Interfaces.Repositories;

public interface IDepositRepository
{
    Task<List<DepositDTO>> GetAll();
    Task<DepositDTO> Add(CreateDepositModel model);
    Task<bool> ExceedsOperationalLimitForCurrentAccount(int accountId, decimal amount, DateTime transactionDate);
}
