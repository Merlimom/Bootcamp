using Core.Constants;
using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class UpdateAccountValidation : AbstractValidator<UpdateAccountModel>
{
    public UpdateAccountValidation()
    {
        RuleFor(x => x.Holder)
            .NotNull().WithMessage("Holder cannot be null")
            .NotEmpty().WithMessage("Holder cannot be empty");

        RuleFor(x => x.Number)
            .NotNull().WithMessage("Card Number cannot be null")
            .NotEmpty().WithMessage("Card Number cannot be empty");

        RuleFor(x => x.Balance)
            .NotNull().WithMessage("Balance cannot be null")
            .NotEmpty().WithMessage("Balance cannot be empty");

        RuleFor(x => x.AccountStatus)
           .Must(x => Enum.IsDefined(typeof(EAccountStatus), x))
           .WithMessage("Invalid Account Status");

        RuleFor(x => x.CustomerId)
            .NotNull().WithMessage("Customer Id cannot be null")
            .NotEmpty().WithMessage("Customer Id cannot be empty");

        RuleFor(x => x.CurrencyId)
            .NotNull().WithMessage("Currency Id cannot be null")
            .NotEmpty().WithMessage("Currency Id cannot be empty");
    }

    private bool BeValidAccountStatus(int arg)
    {
        return Enum.IsDefined(typeof(EAccountStatus), arg);
    }
}
