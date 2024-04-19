using Core.Models;
using Core.Request;

namespace Core.Interfaces.Repositories;

public interface IUserRequestRepository
{
    Task<List<UserRequestDTO>> GetAll();
    Task<UserRequestDTO> Add(CreateUserRequestModel model);
    Task<(bool customerExists, bool currencyExists)> VerifyCustomerAndCurrencyExist(int customerId, int currencyId);
}