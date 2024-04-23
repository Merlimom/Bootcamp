using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
using FluentValidation;

namespace Infrastructure.Services;

public class WithdrawalService : IWithdrawalService
{
    private readonly IWithdrawalRepository _withdrawalRepository;

    public WithdrawalService(IWithdrawalRepository withdrawalRepository)
    {
        _withdrawalRepository = withdrawalRepository;
    }

    public async Task<WithdrawalDTO> Add(CreateWithdrawalModel model)
    {
        bool exceedsLimit = await _withdrawalRepository.ExceedsOperationalLimitForCurrentAccount(model.AccountId, model.Amount);

        // Si excede el límite operacional, lanzar una excepción
        if (exceedsLimit)
        {
            throw new BusinessLogicException("Deposit exceeds the operational limit for the destination account.");
        }
        return await _withdrawalRepository.Add(model);
    }

    public async Task<List<WithdrawalDTO>> GetAll()
    {
        return await _withdrawalRepository.GetAll();
    }
}