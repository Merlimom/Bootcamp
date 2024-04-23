using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
using FluentValidation;

namespace Infrastructure.Services;

public class DepositService : IDepositService
{
    private readonly IDepositRepository _depositRepository;

    public DepositService(IDepositRepository depositRepository)
    {
        _depositRepository = depositRepository;
    }

    public async Task<DepositDTO> Add(CreateDepositModel model)
    {

        bool exceedsLimit = await _depositRepository.ExceedsOperationalLimitForCurrentAccount(model.AccountId, model.Amount);

        // Si excede el límite operacional, lanzar una excepción
        if (exceedsLimit)
        {
            throw new ValidationException("Deposit exceeds the operational limit for the destination account.");
        }

        return await _depositRepository.Add(model);
    }

    public async Task<List<DepositDTO>> GetAll()
    {
        return await _depositRepository.GetAll();
    }
}