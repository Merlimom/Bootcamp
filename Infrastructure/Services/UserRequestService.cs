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
        var (customerExists, currencyExists) = await _userRequestRepository.VerifyCustomerAndCurrencyExist(model.CustomerId, model.CurrencyId);

        if (!customerExists)
        {
            throw new BusinessLogicException($"Customer {model.CustomerId} does not exist.");
        }

        if (!currencyExists)
        {
            throw new BusinessLogicException($"Currency {model.CurrencyId} does not exist.");
        }

        var productExists = await _userRequestRepository.VerifyProductExists(model.ProductId);
        if (!productExists)
        {
            throw new BusinessLogicException($"Product {model.ProductId} does not exist.");
        }
        return await _userRequestRepository.Add(model);
    }

    public async Task<List<UserRequestDTO>> GetAll()
    {
        return await _userRequestRepository.GetAll();
    }
}
