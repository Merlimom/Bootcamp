using Core.Constants;
using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class CreateAccountModelValidation : AbstractValidator<CreateAccountModel>
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
            .Must(IsValidAccountType).WithMessage("Invalid Account Type");

        RuleFor(x => x.CustomerId)
            .NotNull().WithMessage("Customer Id cannot be null")
            .NotEmpty().WithMessage("Customer Id cannot be empty");

        RuleFor(x => x.CurrencyId)
            .NotNull().WithMessage("Currency Id cannot be null")
            .NotEmpty().WithMessage("Currency Id cannot be empty");

        When(x => x.AccountType == AccountType.Saving, () =>
        {
            RuleFor(x => x.CreateSavingAccountModel.SavingType)
                 .NotNull().WithMessage("SavingType is required for Savings account");
        });

        When(x => x.AccountType == AccountType.Current, () =>
        {
            RuleFor(x => x.CreateCurrentAccountModel.OperationalLimit)
                .NotEmpty().WithMessage("OperationalLimit is required for Current account");

            RuleFor(x => x.CreateCurrentAccountModel.MonthAverage)
                .NotEmpty().WithMessage("MonthAverage is required for Current account");

            RuleFor(x => x.CreateCurrentAccountModel.Interest)
                .NotEmpty().WithMessage("Interest is required for Current account");
        });
    }

    private bool BeValidAccountStatus(int arg)
    {
        return Enum.IsDefined(typeof(AccountStatus), arg);
    }

    private bool IsValidAccountType(AccountType accountType)
    {
        return accountType == AccountType.Saving || accountType == AccountType.Current;
    }
}
