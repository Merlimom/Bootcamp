using Core.Models;
using Core.Request;

namespace Core.Interfaces.Services;

public interface ICreditCardService
{
    Task<List<CreditCardDTO>> GetFiltered(FilterCreditCardModel filter);
    Task<CreditCardDTO> Add(CreateCreditCardModel model);
    Task<CreditCardDTO> GetById(int id);
    Task<CreditCardDTO> Update(UpdateCreditCardModel model);
    Task<bool> Delete(int id);
}
