using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
using Infrastructure.Repositories;

namespace Infrastructure.Services;


public class CurrencyService : ICurrencyService
{
    private readonly ICurrencyRepository _currencyRepository;

    public CurrencyService(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<CurrencyDTO> Add(CreateCurrencyModel model)
    {
        return await _currencyRepository.Add(model);
    }

    public async Task<bool> Delete(int id)
    {
        return await _currencyRepository.Delete(id);
    }

    public async Task<CurrencyDTO> GetById(int id)
    {
        return await _currencyRepository.GetById(id);
    }

    public async Task<List<CurrencyDTO>> GetFiltered(FilterCurrencyModel filter)
    {
        return await _currencyRepository.GetFiltered(filter);
    }

    public async Task<CurrencyDTO> Update(UpdateCurrencyModel model)
    {
        return await _currencyRepository.Update(model);
    }
}
