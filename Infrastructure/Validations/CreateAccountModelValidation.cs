using Core.Constants;
using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations; 

public  class CreateAccountModelValidation : AbstractValidator<CreateAccountModel>
{
    public CreateAccountModelValidation()
    {
        RuleFor(x => x.Holder)
             .NotNull().WithMessage("Holder cannot be null")
             .NotEmpty().WithMessage("Holder cannot be empty");

        RuleFor(x => x.Number)
            .NotNull().WithMessage("Card Number cannot be null")
            .NotEmpty().WithMessage("Card Number cannot be empty");

        RuleFor(x => x.AccountType)
            .NotNull().WithMessage("Account Type cannot be null")
            .NotEmpty().WithMessage("Account Type cannot be empty")
            .Must(IsValidAccountType).WithMessage("Invalid Account Type");

        RuleFor(x => x.CustomerId)
            .NotNull().WithMessage("Customer Id cannot be null")
            .NotEmpty().WithMessage("Customer Id cannot be empty");

        RuleFor(x => x.CurrencyId)
            .NotNull().WithMessage("Currency Id cannot be null")
            .NotEmpty().WithMessage("Currency Id cannot be empty");

        //Validaciones adicionales para la cuenta de ahorro
        When(x => x.AccountType == "Saving", () =>
        {
            RuleFor(x => x.SavingType)
                .NotNull().WithMessage("SavingType is required for Savings account");

            RuleFor(x => x.HolderName)
                .NotNull().WithMessage("HolderName is required for Savings account")
                .NotEmpty().WithMessage("HolderName cannot be empty for Savings account");
        });

        // Validaciones adicionales para la cuenta corriente
        When(x => x.AccountType == "Current", () =>
        {
            RuleFor(x => x.OperationalLimit)
                .NotNull().WithMessage("OperationalLimit is required for Current account");

            RuleFor(x => x.MonthAverage)
                .NotNull().WithMessage("MonthAverage is required for Current account");

            RuleFor(x => x.Interest)
                .NotNull().WithMessage("Interest is required for Current account");
        });
    }

    private bool BeValidAccountStatus(int arg)
    {
        return Enum.IsDefined(typeof(AccountStatus), arg);
    }

    private bool IsValidAccountType(string accountType)
    {
        return accountType == "Saving" || accountType == "Current";
    }
}
