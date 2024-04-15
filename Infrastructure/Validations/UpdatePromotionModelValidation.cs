using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class UpdatePromotionModelValidation : AbstractValidator<UpdatePromotionModel>
{
    public UpdatePromotionModelValidation()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Account Id cannot be empty")
            .Must(x => x > 0).WithMessage("Invalid Account Id");

        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name cannot be null")
            .NotEmpty().WithMessage("Name cannot be empty")
            .MinimumLength(5).WithMessage("Name must have at least 5 characters");

        RuleFor(x => x.BusinessId)
            .NotNull().WithMessage("BusinessId cannot be null")
            .NotEmpty().WithMessage("BusinessId cannot be empty");
    }
}
