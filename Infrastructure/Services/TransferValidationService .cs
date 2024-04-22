﻿
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Request;
using FluentValidation;

namespace Infrastructure.Services;

public class TransferValidationService : ITransferValidationService
{
    private readonly IMovementRepository _movementRepository;

    public TransferValidationService(IMovementRepository movementRepository)
    {
        _movementRepository = movementRepository;
    }

    public async Task<bool> ValidateTransfer(int accountSourceId, int accountDestinationId, decimal amount,
                                             int? destinationBankId, string? destinationAccountNumber, string? destinationDocumentNumber, int? currencyId)
    {
        if (!await _movementRepository.IsSameAccountType(accountSourceId, accountDestinationId))
        {
            throw new ValidationException("The accounts are not the same type.");
        }

        if (!await _movementRepository.IsSameCurrency(accountSourceId, accountDestinationId))
        {
            throw new ValidationException("The accounts do not have the same currency.");
        }

        if (!await _movementRepository.IsSufficientBalance(accountSourceId, amount))
        {
            throw new ValidationException("The source account does not have sufficient balance.");
        }

        if (!await _movementRepository.IsSourceAccountActive(accountSourceId))
        {
            throw new ValidationException("Source account is not active.");
        }

        var (exceedsLimit, accountType) = await _movementRepository.ExceedsOperationalLimit(accountSourceId, accountDestinationId, amount);

        if (exceedsLimit)
        {
            var accountName = accountType == "source" ? "source account" : "destination account";
            throw new ValidationException($"The transfer exceeds the operational limit of the {accountName}.");
        }

        if (!await _movementRepository.IsSameBank(accountSourceId, accountDestinationId))
        {
            if (destinationBankId == null || destinationBankId == 0 ||
                currencyId == null || currencyId == 0 ||
                string.IsNullOrEmpty(destinationAccountNumber) || destinationAccountNumber == "string" ||
                string.IsNullOrEmpty(destinationDocumentNumber) || destinationDocumentNumber == "string")
            {
                throw new ValidationException("When transferring between different banks, 'Destination Bank Id' and 'Currency Id' must be greater than 0, " +
                                              "'Destination Account Number' and 'Destination Document Number' must be valid values.");
            }
        }

        return true;
    }
}