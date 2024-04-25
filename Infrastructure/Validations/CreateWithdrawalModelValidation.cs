using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class CreateWithdrawalModelValidation : AbstractValidator<CreateWithdrawalModel>
{
    public CreateWithdrawalModelValidation()
    {
        RuleFor(x => x.AccountId)
         .NotNull().WithMessage("Amount cannot be null")
         .NotEmpty().WithMessage("Amount cannot be empty");

        RuleFor(x => x.BankId)
          .NotNull().WithMessage("Amount cannot be null")
          .NotEmpty().WithMessage("Amount cannot be empty");

        RuleFor(x => x.Amount)
         .NotNull().WithMessage("Amount cannot be null")
         .NotEmpty().WithMessage("Amount cannot be empty")
         .GreaterThan(0).WithMessage("Credit Limit must be greater than zero.");

    }
}