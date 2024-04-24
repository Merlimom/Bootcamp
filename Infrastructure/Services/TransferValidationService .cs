
using Core.Exceptions;
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
                                             int? destinationBankId, string? destinationAccountNumber, string? destinationDocumentNumber, int? currencyId, DateTime TransferredDateTime)
    {
        if (!await _movementRepository.IsSameAccountType(accountSourceId, accountDestinationId))
        {
            throw new BusinessLogicException("The accounts are not the same type.");
        }

        if (!await _movementRepository.IsSameCurrency(accountSourceId, accountDestinationId))
        {
            throw new BusinessLogicException("The accounts do not have the same currency.");
        }

        if (!await _movementRepository.IsSufficientBalance(accountSourceId, amount))
        {
            throw new BusinessLogicException("The source account does not have sufficient balance.");
        }

        if (!await _movementRepository.IsSourceAccountActive(accountSourceId))
        {
            throw new BusinessLogicException("Source account is not active.");
        }

        (bool exceedsLimit, string accountType) = await _movementRepository.ExceedsOperationalLimit(accountSourceId, accountDestinationId, amount, TransferredDateTime);

        if (exceedsLimit)
        {
            var accountName = accountType == "source" ? "source account" : "destination account";
            throw new BusinessLogicException($"The transfer exceeds the operational limit of the {accountName}.");
        }


        if (!await _movementRepository.IsSameBank(accountSourceId, accountDestinationId))
        {
            if (destinationBankId == null || destinationBankId == 0 ||
                currencyId == null || currencyId == 0 ||
                string.IsNullOrEmpty(destinationAccountNumber) || destinationAccountNumber == "string" ||
                string.IsNullOrEmpty(destinationDocumentNumber) || destinationDocumentNumber == "string")
            {
                throw new BusinessLogicException("When transferring between different banks, 'Destination Bank Id' and 'Currency Id' must be greater than 0, " +
                                              "'Destination Account Number' and 'Destination Document Number' must be valid values.");
            }
        }

        return true;
    }
}
