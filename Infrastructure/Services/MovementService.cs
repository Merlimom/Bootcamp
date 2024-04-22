
using Core.Constants;
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
                                                                 model.DestinationAccountNumber, model.CurrencyId);
        if (!isValid)
        {
            throw new ValidationException("Movement validation failed.");
        }

        //if (model.MovementType == EMovementType.Deposit)
        //{
        //    await _movementRepository.ProcessDeposit(model);
        //}

        //if (model.MovementType == EMovementType.Withdrawal)
        //{
        //    await _movementRepository.ProcessWithdrawal(model);
        //}

        return await _movementRepository.Add(model);
    }

    public async Task<List<MovementDTO>> GetAll()
    {
        return await _movementRepository.GetAll();
    }

}
