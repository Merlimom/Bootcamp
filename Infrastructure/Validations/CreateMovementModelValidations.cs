using Core.Constants;
using Core.Entities;
using Core.Request;
using FluentValidation;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Validations;

public class CreateMovementModelValidations : AbstractValidator<CreateMovementModel>
{
    public CreateMovementModelValidations()
    {
        RuleFor(x => x.MovementType)
            .NotNull().WithMessage("Movement Type cannot be null")
            .Must(IsValidMovementType).WithMessage("Invalid Movement Type");

        RuleFor(x => x.Amount)
            .NotNull().WithMessage("Amount cannot be null")
            .NotEmpty().WithMessage("Amount cannot be empty");

       // RuleFor(x => x.AccountSourceId)
       //      .Must((model, accountSourceId) => IsSameAccountType(model))
       //      .WithMessage("Source and Destination Accounts must be of the same type");

       // RuleFor(x => x.AccountSourceId)
       //     .Must((model, accountSourceId) => IsSameCurrency(model))
       //     .WithMessage("Source and Destination Accounts must have the same currency");

       // RuleFor(x => x.Amount)
       //     .Must((model, amount) => IsNotExceedingBalance(model))
       //     .WithMessage("Transfer amount cannot exceed account balance");

       // RuleFor(x => x.AccountSourceId)
       //     .Must((model, accountSourceId) => IsAccountActive(model))
       //     .WithMessage("Source Account must be active");

       // RuleFor(x => x.Amount)
       //        .Must((model, amount) => IsNotExceedingOperationalLimit(model))
       //        .WithMessage("Transfer amount exceeds operational limit");

       // RuleFor(x => x.AccountSourceId)
       //     .Must((model, accountSourceId) => IsSameBank(model))
       //     .WithMessage("Transfer between accounts of the same bank is required");

       // RuleFor(x => x.AccountDestinationId)
       //.NotEmpty().WithMessage("Destination bank is required.")
       //.When(x => !IsSameBank(x)).WithMessage("Destination bank is required.");

       // RuleFor(x => x.DestinationAccountNumber)
       //     .NotEmpty().WithMessage("Destination account number is required.")
       //     .When(x => !IsSameBank(x)).WithMessage("Destination account number is required.");

       // RuleFor(x => x.DestinationDocumentNumber)
       //     .NotEmpty().WithMessage("Destination document number is required.")
       //     .When(x => !IsSameBank(x)).WithMessage("Destination document number is required.");

       // RuleFor(x => x.DestinationCurrency)
       //     .NotEmpty().WithMessage("Destination currency is required.")
       //     .When(x => !IsSameBank(x)).WithMessage("Destination currency is required.");

    }

    private bool IsValidMovementType(EMovementType movementType)
    {
        return movementType == EMovementType.Transfer
            || movementType == EMovementType.Deposit
            || movementType == EMovementType.Withdrawal;
    }


    //private bool IsSameAccountType(CreateMovementModel model)
    //{
    //    AccountType accountTypeSource = GetAccountType(model.AccountSourceId);
    //    AccountType accountTypeDestination = GetAccountType(model.AccountDestinationId);

    //    return accountTypeSource == accountTypeDestination;
    //}

    //private AccountType GetAccountType(int accountId)
    //{
    //    using (var context = new BootcampContext())
    //    {
    //        var account = context.Accounts.FirstOrDefault(a => a.Id == accountId);
    //        if (account != null)
    //        {
    //            return account.AccountType;
    //        }
    //    }

    //    throw new Exception($"No account was found with the specified ID: {accountId}");
    //}

    //private bool IsSameCurrency(CreateMovementModel model)
    //{
    //    Currency currencySource = GetCurrencyForAccount(model.AccountSourceId);

    //    Currency currencyDestination = GetCurrencyForAccount(model.AccountDestinationId);

    //    return currencySource == currencyDestination;
    //}

    //private Currency GetCurrencyForAccount(int accountId)
    //{
    //    using (var context = new BootcampContext())
    //    {
    //        var account = context.Accounts
    //                            .Include(a => a.Currency)
    //                            .FirstOrDefault(a => a.Id == accountId);

    //        if (account != null)
    //        {
    //            return account.Currency;
    //        }
    //    }

    //    throw new Exception($"No account was found with the specified ID: {accountId}");
    //}

    //private bool IsNotExceedingBalance(CreateMovementModel model)
    //{
    //    decimal accountBalance = GetAccountBalance(model.AccountSourceId);

    //    return accountBalance >= model.Amount;
    //}

    //private decimal GetAccountBalance(int accountId)
    //{
    //    using (var context = new BootcampContext())
    //    {
    //        var account = context.Accounts.FirstOrDefault(a => a.Id == accountId);
    //        if (account != null)
    //        {
    //            return account.Balance;
    //        }
    //    }

    //    throw new Exception($"No account was found with the specified ID: {accountId}");
    //}


    //private bool IsAccountActive(CreateMovementModel model)
    //{
    //    Account? account = GetAccount(model.AccountSourceId);
    //    return account != null && account.AccountStatus == AccountStatus.Active;
    //}

    //private Account? GetAccount(int accountId)
    //{
    //    using (var context = new BootcampContext())
    //    {
    //        var account = context.Accounts.FirstOrDefault(a => a.Id == accountId);

    //        if (account == null)
    //        {
    //            Console.WriteLine($"No account was found with the specified ID: {accountId}");
    //            return null;
    //        }

    //        return account;
    //    }
    //}

    //private bool IsSameBank(CreateMovementModel model)
    //{
    //    Bank bankSource = GetBankForAccount(model.AccountSourceId);

    //    Bank bankDestination = GetBankForAccount(model.AccountDestinationId);

    //    return bankSource != null && bankDestination != null && bankSource.Id == bankDestination.Id;
    //}

    //private Bank GetBankForAccount(int accountId)
    //{
    //    using (var context = new BootcampContext())
    //    {
    //        var account = context.Accounts
    //                            .Include(a => a.Customer)
    //                            .ThenInclude(c => c.Bank) 
    //                            .FirstOrDefault(a => a.Id == accountId);

    //        if (account != null && account.Customer != null && account.Customer.Bank != null)
    //        {
    //            return account.Customer.Bank; 
    //        }

    //        throw new Exception($"No bank was found for the account with ID: {accountId}");
    //    }

    //}

    //private bool IsNotExceedingOperationalLimit(CreateMovementModel model)
    //{
    //    // Obtener la cuenta de origen
    //    Account? account = GetAccount(model.AccountSourceId);

    //    // Verificar si la cuenta existe y si es una cuenta de tipo "current account" con un límite operacional definido
    //    if (account != null && account.AccountType == AccountType.Current && account.CurrentAccount != null)
    //    {
    //        // Obtener el límite operacional de la cuenta
    //        decimal operationalLimit = account.CurrentAccount.OperationalLimit;

    //        // Comparar el monto de la transferencia con el límite operacional
    //        if (model.Amount > operationalLimit)
    //        {
    //            // Si el monto de la transferencia excede el límite operacional, lanzar una excepción
    //            throw new Exception("Transfer amount exceeds operational limit");
    //        }

    //        // Si el monto de la transferencia no excede el límite operacional, devolver true
    //        return true;
    //    }

    //    // Si la cuenta no existe, no es una cuenta de tipo "current account" o no tiene un límite operacional definido,
    //    // simplemente devolver true ya que no se aplica un límite operacional en este caso
    //    return true;
    //}
    
}
