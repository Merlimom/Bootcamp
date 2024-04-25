using Core.Models;
using Core.Request;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Infrastructure.Services
{
    public class AccountTransactionService : IAccountTransactionService
    {
        private readonly IAccountTransactionRepository _accountTransactionRepository;

        public AccountTransactionService(IAccountTransactionRepository accountTransactionRepository)
        {
            _accountTransactionRepository = accountTransactionRepository;
        }

        public async Task<List<AccountTransactionsDTO>> GetFilteredAccountTransactions(FilterTransactionModel filters)
        {
            return await _accountTransactionRepository.GetFilteredAccountTransactions(filters);
        }
    }
}
