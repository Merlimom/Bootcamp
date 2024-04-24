namespace Core.Interfaces.Services
{
    public interface ITransferValidationService
    {
        Task<bool> ValidateTransfer(int accountSourceId, int accountDestinationId,
                                    decimal amount, int? destinationBankId, string? destinationAccountNumber,
                                    string? destinationDocumentNumber, int? currencyId, DateTime transactionDate);
    }
}