
using Core.Constants;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Request;
using FluentValidation;

namespace Infrastructure.Services;

public class MovementService : IMovementService
{
    private readonly IMovementRepository _movementRepository;
    private readonly ITransferValidationService _validationService;

    public MovementService(IMovementRepository movementRepository, ITransferValidationService validationService)
    {
        _movementRepository = movementRepository; // Inyectado
        _validationService = validationService; // Inyectado
    }
    public async Task<MovementDTO> Add(CreateMovementModel model)
    {
        bool isValid = await _validationService.ValidateTransfer(model.AccountSourceId, model.AccountDestinationId, model.Amount,
                                                                 model.DestinationBankId, model.DestinationDocumentNumber,
                                                                 model.DestinationAccountNumber, model.CurrencyId, model.TransferredDateTime.HasValue ? model.TransferredDateTime.Value : default);
        if (!isValid)
        {
            throw new BusinessLogicException("Movement validation failed.");
        }


        return await _movementRepository.Add(model);
    }

    public async Task<List<MovementDTO>> GetAll()
    {
        return await _movementRepository.GetAll();
    }

}
