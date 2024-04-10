using Core.Request;
using FluentValidation;

namespace Infrastructure.Validations;

public class UpdateCurrencyModelValidation : AbstractValidator<UpdateCurrencyModel>
{
    public UpdateCurrencyModelValidation()
    {
        RuleFor(x => x.Name)
          .NotNull().WithMessage("Name cannot be null")
          .NotEmpty().WithMessage("Name cannot be empty");

        RuleFor(x => x.BuyValue)
           .NotNull().WithMessage("Name cannot be null")
           .NotEmpty().WithMessage("Name cannot be empty");

        RuleFor(x => x.SellValue)
           .NotNull().WithMessage("Name cannot be null")
           .NotEmpty().WithMessage("Name cannot be empty");
    }
}
