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
       
        return await _withdrawalRepository.Add(model);
    }

    public async Task<List<WithdrawalDTO>> GetAll()
    {
        return await _withdrawalRepository.GetAll();
    }
}