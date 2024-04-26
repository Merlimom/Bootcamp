using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class UpdatePromotionModelValidation : AbstractValidator<UpdatePromotionModel>
{
    public UpdatePromotionModelValidation()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(" Id cannot be empty")
            .Must(x => x > 0).WithMessage("Invalid  Id");

        RuleFor(x => x.Name)
                  .NotNull().WithMessage("Name cannot be null")
                  .NotEmpty().WithMessage("Name cannot be empty");

        RuleFor(x => x.Start)
           .NotNull().WithMessage("Start cannot be null")
           .NotEmpty().WithMessage("Start cannot be empty");

        RuleFor(x => x.End)
           .NotNull().WithMessage("End cannot be null")
           .NotEmpty().WithMessage("End cannot be empty");

        RuleFor(x => x.Discount)
           .NotNull().WithMessage("Discount cannot be null")
           .NotEmpty().WithMessage("Discount cannot be empty");
    }
}
