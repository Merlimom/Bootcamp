using Core.Models;
using Core.Request;

namespace Core.Interfaces.Repositories;

public interface IWithdrawalRepository
{
    Task<List<WithdrawalDTO>> GetAll();
    Task<WithdrawalDTO> Add(CreateWithdrawalModel model);
    Task<bool> DoesAccountExist(int accountId);
    Task<bool> DoesBankExist(int bankId);

}