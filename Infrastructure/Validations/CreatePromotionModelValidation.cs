using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;
    public class CreatePromotionModelValidation : AbstractValidator<CreatePromotionModel>
    {
        public CreatePromotionModelValidation()
        {
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

