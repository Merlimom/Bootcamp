using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class UserRequestService : IUserRequestService
{
    private readonly IUserRequestRepository _userRequestRepository;

    public UserRequestService(IUserRequestRepository userRequestRepository)
    {
        _userRequestRepository = userRequestRepository;
    }

    public async Task<UserRequestDTO> Add(CreateUserRequestModel model)
    {
        bool customerDoesntExist = await _userRequestRepository.VerifyCustomerExists(model.CustomerId);
        if (customerDoesntExist)
        {
            throw new BusinessLogicException($"Customer {model.CustomerId} does not exist");
        }

        bool currencyDoesntExist = await _userRequestRepository.VerifyCurrencyExists(model.CurrencyId);
        if (currencyDoesntExist)
        {
            throw new BusinessLogicException($"Currency {model.CurrencyId} does not exist");
        }
        return await _userRequestRepository.Add(model);
    }

    public async Task<List<UserRequestDTO>> GetAll()
    {
        return await _userRequestRepository.GetAll();
    }
}
