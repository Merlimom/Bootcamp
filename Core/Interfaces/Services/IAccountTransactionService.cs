using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using Core.Request;

namespace Core.Interfaces.Services
{
    public interface IAccountTransactionService
    {
        Task<List<AccountTransactionsDTO>> GetFilteredAccountTransactions(FilterTransactionModel filters);
    }
}
