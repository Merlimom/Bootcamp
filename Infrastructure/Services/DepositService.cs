using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
using FluentValidation;
using Infrastructure.Repositories;

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

        // Verificar si la cuenta existe
        var accountExists = await _depositRepository.DoesAccountExist(model.AccountId);
        if (!accountExists)
        {
            throw new NotFoundException("Account does not exist.");
        }

        // Verificar si el banco existe
        var bankExists = await _depositRepository.DoesBankExist(model.BankId);
        if (!bankExists)
        {
            throw new NotFoundException("Bank does not exist.");
        }

        bool exceedsLimit = await _depositRepository.ExceedsOperationalLimitForCurrentAccount(model.AccountId, model.Amount, model.DepositDateTime);

        // Si excede el límite operacional, lanzar una excepción
        if (exceedsLimit)
        {
            throw new BusinessLogicException("Deposit exceeds the operational limit for the destination account.");
        }


        return await _depositRepository.Add(model);
    }

    public async Task<List<DepositDTO>> GetAll()
    {
        return await _depositRepository.GetAll();
    }
}