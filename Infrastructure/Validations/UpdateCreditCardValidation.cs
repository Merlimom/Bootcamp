using Core.Constants;
using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class UpdateCreditCardValidation : AbstractValidator<UpdateCreditCardModel>
{
    public UpdateCreditCardValidation()
    {
        RuleFor(x => x.Designation)
         .NotNull().WithMessage("Designation cannot be null")
         .NotEmpty().WithMessage("Designation cannot be empty");

        RuleFor(x => x.IssueDate)
            .NotNull().WithMessage("Issue Date cannot be null")
            .NotEmpty().WithMessage("Issue Date cannot be empty");

        RuleFor(x => x.ExpirationDate)
            .NotNull().WithMessage("Expiration Date cannot be null")
            .NotEmpty().WithMessage("Expiration Date cannot be empty");

        RuleFor(x => x.CardNumber)
            .NotNull().WithMessage("Card Number cannot be null")
            .NotEmpty().WithMessage("Card Number cannot be empty");

        RuleFor(x => x.CreditLimit)
          .NotNull().WithMessage("Credit Limit cannot be null")
          .GreaterThan(0).WithMessage("Credit Limit must be greater than zero.");

        RuleFor(x => x.AvailableCredit)
            .NotNull().WithMessage("Available Credit cannot be null")
            .GreaterThan(500000).WithMessage("Interest must be greater than five hundred thousand.");

        RuleFor(x => x.InterestRate)
          .NotNull().WithMessage("Interest Rate cannot be null")
          .GreaterThan(0).WithMessage("Interest must be greater than zero.");

        RuleFor(x => x.CustomerId)
          .NotNull().WithMessage("Customer Id cannot be null")
          .NotEmpty().WithMessage("Customer Id cannot be empty");
    }

    private bool BeValidCreditCardStatus(int arg)
    {
        return Enum.IsDefined(typeof(CreditCardStatus), arg);
    }

    private bool IsValidCreditCardNumber(string creditCardNumber)
    {
        return creditCardNumber.Length == 16;
    }
}
