using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class CreditCardService : ICreditCardService
{
    private readonly ICreditCardRepository _creditCardRepository;

    public CreditCardService(ICreditCardRepository creditCardRepository)
    {
        _creditCardRepository = creditCardRepository;
    }


    public async Task<CreditCardDTO> Add(CreateCreditCardModel model)
    {
        //bool customerDoesntExist = await _creditCardRepository.VerifyCustomerExists(model.CustomerId);
        //if (customerDoesntExist)
        //{
        //    throw new BusinessLogicException($"Customer {model.CustomerId} does not exist");
        //}

        //bool currencyDoesntExist = await _creditCardRepository.VerifyCurrencyExists(model.CurrencyId);
        //if (currencyDoesntExist)
        //{
        //    throw new BusinessLogicException($"Currency {model.CurrencyId} does not exist");
        //}

        return await _creditCardRepository.Add(model);
    }


    public async Task<bool> Delete(int id)
    {
        return await _creditCardRepository.Delete(id);
    }

    public async Task<CreditCardDTO> GetById(int id)
    {
        return await _creditCardRepository.GetById(id);
    }

    public async Task<List<CreditCardDTO>> GetFiltered(FilterCreditCardModel filter)
    {
        return await _creditCardRepository.GetFiltered(filter);
    }

    public async Task<CreditCardDTO> Update(UpdateCreditCardModel model)
    {
        return await _creditCardRepository.Update(model);
    }
}
