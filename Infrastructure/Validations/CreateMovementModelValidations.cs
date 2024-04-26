using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class CreateMovementModelValidations : AbstractValidator<CreateMovementModel>
{
    public CreateMovementModelValidations()
    {
        RuleFor(x => x.Amount)
            .NotNull().WithMessage("Amount cannot be null")
            .NotEmpty().WithMessage("Amount cannot be empty")
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

    }
}
