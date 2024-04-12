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
        bool customerDoesntExist = await _accountRepository.VerifyCustomerExists(model.CustomerId);
        if (customerDoesntExist)
        {
            throw new BusinessLogicException($"Customer {model.CustomerId} does not exist");
        }

        bool currencyDoesntExist = await _accountRepository.VerifyCurrencyExists(model.CurrencyId);
        if (currencyDoesntExist)
        {
            throw new BusinessLogicException($"Currency {model.CurrencyId} does not exist");
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
