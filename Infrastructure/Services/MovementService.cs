using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class MovementService : IMovementService
{
    private readonly IMovementRepository _movementRepository;

    public MovementService(IMovementRepository movementRepository)
    {
        _movementRepository = movementRepository;
    }

    public async Task<MovementDTO> Add(CreateMovementModel model)
    {
        return await _movementRepository.Add(model);
    }

    public async Task<List<MovementDTO>> GetAll()
    {
        return await _movementRepository.GetAll();
    }
}
