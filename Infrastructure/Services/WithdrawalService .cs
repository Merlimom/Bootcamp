using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
using FluentValidation;
using Infrastructure.Repositories;

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
        // Verificar si la cuenta existe
        var accountExists = await _withdrawalRepository.DoesAccountExist(model.AccountId);
        if (!accountExists)
        {
            throw new NotFoundException("Account does not exist.");
        }

        // Verificar si el banco existe
        var bankExists = await _withdrawalRepository.DoesBankExist(model.BankId);
        if (!bankExists)
        {
            throw new NotFoundException("Bank does not exist.");
        }

        return await _withdrawalRepository.Add(model);
    }

    public async Task<List<WithdrawalDTO>> GetAll()
    {
        return await _withdrawalRepository.GetAll();
    }
}