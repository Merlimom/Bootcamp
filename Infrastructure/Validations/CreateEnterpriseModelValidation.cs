using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class CreateEnterpriseModelValidation : AbstractValidator<CreateEnterpriseModel>
{
    public CreateEnterpriseModelValidation()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name cannot be null")
            .NotEmpty().WithMessage("Name cannot be empty");

        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.Phone)
            .NotNull().WithMessage("Phone cannot be null")
            .NotEmpty().WithMessage("Phone cannot be empty");
    }
}
