using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class CreateWithdrawalModelValidation : AbstractValidator<CreateWithdrawalModel>
{
    public CreateWithdrawalModelValidation()
    {
        RuleFor(x => x.AccountId)
         .NotEmpty().WithMessage("AccountId cannot be empty");

        RuleFor(x => x.BankId)
          .NotEmpty().WithMessage("BankId cannot be empty");

        RuleFor(x => x.Amount)
         .NotNull().WithMessage("Amount cannot be null")
         .NotEmpty().WithMessage("Amount cannot be empty")
         .GreaterThan(0).WithMessage("Amount must be greater than zero.");

    }
}