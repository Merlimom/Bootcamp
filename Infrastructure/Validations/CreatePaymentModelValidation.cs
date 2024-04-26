using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class CreatePaymentValidation : AbstractValidator<CreatePaymentModel>
{
    public CreatePaymentValidation()
    {
        RuleFor(x => x.DocumentNumber)
            .NotNull().WithMessage("DocumentNumber cannot be null")
            .NotEmpty().WithMessage("DocumentNumber cannot be empty");

        RuleFor(x => x.Description)
          .NotNull().WithMessage("Description cannot be null")
          .NotEmpty().WithMessage("Description cannot be empty");


        RuleFor(x => x.Amount)
         .NotNull().WithMessage("Amount cannot be null")
         .NotEmpty().WithMessage("Amount cannot be empty")
         .GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }
}