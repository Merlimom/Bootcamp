using Core.Models;
using Core.Request;

namespace Core.Interfaces.Services; 

public interface ICurrencyService
{
    Task<List<CurrencyDTO>> GetFiltered(FilterCurrencyModel filter);
    Task<CurrencyDTO> Add(CreateCurrencyModel model);
    Task<CurrencyDTO> GetById(int id);
    Task<CurrencyDTO> Update(UpdateCurrencyModel model);
    Task<bool> Delete(int id);
    Task<List<CurrencyDTO>> GetAll();
}
