using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
       _accountRepository = accountRepository;
    }

    public async Task<AccountDTO> Add(CreateAccountModel model)
    {
        var (customerExists, currencyExists) = await _accountRepository.VerifyCustomerAndCurrencyExist(model.CustomerId, model.CurrencyId);

        if (!customerExists)
        {
            throw new BusinessLogicException($"Customer {model.CustomerId} does not exist.");
        }

        if (!currencyExists)
        {
            throw new BusinessLogicException($"Currency {model.CurrencyId} does not exist.");
        }
        return await _accountRepository.Add(model);
    }

    public async Task<bool> Delete(int id)
    {
        return await _accountRepository.Delete(id);  
    }

    public async Task<List<AccountDTO>> GetFiltered(FilterAccountModel filter)
    {
        return await _accountRepository.GetFiltered(filter);
    }

    public async Task<AccountDTO> Update(UpdateAccountModel model)
    {
        return await _accountRepository.Update(model);
    }
}
