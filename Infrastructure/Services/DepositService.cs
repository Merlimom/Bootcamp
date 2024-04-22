using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
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
        return await _depositRepository.Add(model);
    }

    public async Task<List<DepositDTO>> GetAll()
    {
        return await _depositRepository.GetAll();
    }
}