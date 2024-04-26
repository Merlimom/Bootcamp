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

        RuleFor(x => x.Cvv)
          .NotNull().WithMessage("Card Number cannot be null")
          .NotEmpty().WithMessage("Card Number cannot be empty")
          .Must(IsValidCvvNumber).WithMessage("Card number must have 3 numeric digits");

        RuleFor(x => x.CreditCardStatus)
            .Must(x => Enum.IsDefined(typeof(ECreditCardStatus), x))
            .WithMessage("Invalid CreditCard Status");

        RuleFor(x => x.CreditLimit)
          .NotNull().WithMessage("Credit Limit cannot be null")
          .GreaterThan(0).WithMessage("Credit Limit must be greater than zero.");

        RuleFor(x => x.AvailableCredit)
            .NotNull().WithMessage("Available Credit cannot be null")
            .GreaterThan(500000).WithMessage("Available Credit must be greater than five hundred thousand.");

        RuleFor(x => x.InterestRate)
          .NotNull().WithMessage("Interest Rate cannot be null")
          .GreaterThan(0).WithMessage("Interest must be greater than zero.");

        RuleFor(x => x.CustomerId)
          .NotEmpty().WithMessage("Customer Id cannot be empty");
    }

    private bool IsValidCvvNumber(int cvvNumber)
    {
        return cvvNumber.ToString().Length == 3;
    }

}
