using Core.Models;
using Core.Request;

namespace Core.Interfaces.Repositories
{
    public interface IAccountTransactionRepository
    {
        Task<List<AccountTransactionsDTO>> GetFilteredAccountTransactions(FilterTransactionModel filters);
    }
}
