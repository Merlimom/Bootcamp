using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class CreatePaymentValidation : AbstractValidator<CreatePaymentModel>
{
    public CreatePaymentValidation()
    {
        RuleFor(x => x.DocumentNumber)
            .NotNull().WithMessage("Amount cannot be null")
            .NotEmpty().WithMessage("Amount cannot be empty");

        RuleFor(x => x.Description)
          .NotNull().WithMessage("Amount cannot be null")
          .NotEmpty().WithMessage("Amount cannot be empty");


        RuleFor(x => x.Amount)
         .NotNull().WithMessage("Amount cannot be null")
         .NotEmpty().WithMessage("Amount cannot be empty")
         .GreaterThan(0).WithMessage("Credit Limit must be greater than zero.");
    }
}