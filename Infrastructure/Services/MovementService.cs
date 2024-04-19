using Core.Constants;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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
        bool isValid = await ValidateTransfer(model);

        if (!isValid)
        {
            throw new Exception("Movement validation failed.");
        }

        // Agregar el movimiento si la validación es exitosa
        var result = await _movementRepository.Add(model);
        return result;
    }


    private async Task<bool> ValidateTransfer(CreateMovementModel model)
    {
        // Llamar a las validaciones necesarias en el repositorio o en otros servicios
        bool isTransferValid = await _movementRepository.ValidateTransfer(model.AccountSourceId, model.AccountDestinationId, model.Amount);
        // Agrega más validaciones según sea necesario

        // Devuelve true si todas las validaciones son exitosas, de lo contrario, devuelve false
        return isTransferValid;
    }

    public async Task<List<MovementDTO>> GetAll()
    {
        return await _movementRepository.GetAll();
    }
}