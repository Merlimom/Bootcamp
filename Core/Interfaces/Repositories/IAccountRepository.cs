using Core.Models;
using Core.Request;

namespace Core.Interfaces.Repositories;

    public interface IAccountRepository
    {
    Task<List<AccountDTO>> GetFiltered(FilterAccountModel filter);
    Task<AccountDTO> Add(CreateAccountModel model);
    Task<AccountDTO> Update(UpdateAccountModel model);
    Task<bool> Delete(int id);
    }

